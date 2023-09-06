using System.IO;
using System.Threading.Tasks;
using Common;
using JsonUtilities;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Newtonsoft.Json;
using OneOf;
using Players;

namespace Remote
{
  /// <summary>
  /// Represents a player-dispatcher that uses a JSON communication protocol to talk with a proxy player.
  /// </summary>
  public class PlayerDispatcher : IPlayerDispatcher
  {
    public async Task<Result> RunAsync(TextReader reader, TextWriter writer, IPlayer player)
    {
      var protectedReader = new ProtectedJsonReader(reader);
      var protectedWriter = new ProtectedJsonWriter(writer);
      var protectedPlayers = new ProtectedPlayer(player);
      while (await protectedReader.ReadAsync())
      {
        Try<OneOf<SetupCall, TakeTurnCall, WonCall>> methodCall = protectedReader.ReadMethodCall();
        OneOf<WriteError, PlayerError, GameStatus> methodResult = await methodCall.Match(
          async method => await InvokeMethodOnPlayerAsync(protectedWriter, protectedPlayers, method),
          _ => Task.FromResult(OneOf<WriteError, PlayerError, GameStatus>.FromT2(new GameStatus(Option<bool>.None)))
        );

        if (ShouldStop(methodResult))
        {
          return ToFinalResult(methodResult);
        }
      }

      return new Result(OneOf<ReadError, WriteError, PlayerError, bool>.FromT0(new ReadError()));
    }

    private static bool ShouldStop(OneOf<WriteError, PlayerError, GameStatus> methodResult)
    {
      return methodResult.Match(
        _ => true,
        _ => true,
        gameResult => gameResult.Value.IsSome
      );
    }

    private static Result ToFinalResult(
      OneOf<WriteError, PlayerError, GameStatus> methodResult)
    {
      return new Result(methodResult.Match(
        OneOf<ReadError, WriteError, PlayerError, bool>.FromT1,
        OneOf<ReadError, WriteError, PlayerError, bool>.FromT2,
        gameStatus => gameStatus.Value.Value()
      ));
    }

    private async Task<OneOf<WriteError, PlayerError, GameStatus>> InvokeMethodOnPlayerAsync(ProtectedJsonWriter writer,
      ProtectedPlayer player, OneOf<SetupCall, TakeTurnCall, WonCall> methodCall)
    {
      return await methodCall.Match(
        async setup => await InvokeSetupAsync(writer, player, setup.State, setup.Goal),
        async takeTurn => await InvokeTakeTurnAsync(writer, player, takeTurn.State),
        async won => await InvokeWonAsync(writer, player, won.Won)
      );
    }

    private async Task<OneOf<WriteError, PlayerError, GameStatus>> InvokeSetupAsync(ProtectedJsonWriter writer,
      ProtectedPlayer player, Option<IPlayerState> maybeState, BoardPosition goal)
    {
      return await player.Setup(maybeState, goal).Match(
        _ => writer.WriteAcknowledgeAsync().Match(
          _ => OneOf<WriteError, PlayerError, GameStatus>.FromT2(new GameStatus(Option<bool>.None)),
          fail => OneOf<WriteError, PlayerError, GameStatus>.FromT0(new WriteError(fail))
        ),
        fail => Task.FromResult(OneOf<WriteError, PlayerError, GameStatus>.FromT1(new PlayerError(fail)))
      );
    }

    private async Task<OneOf<WriteError, PlayerError, GameStatus>> InvokeTakeTurnAsync(ProtectedJsonWriter writer,
      ProtectedPlayer player, IPlayerState state)
    {
      return await player.TakeTurn(state).Match(
        maybeMove => writer.WriteMaybeMoveAsync(maybeMove).Match(
          _ => OneOf<WriteError, PlayerError, GameStatus>.FromT2(new GameStatus(Option<bool>.None)),
          fail => OneOf<WriteError, PlayerError, GameStatus>.FromT0(new WriteError(fail))),
        fail => Task.FromResult(OneOf<WriteError, PlayerError, GameStatus>.FromT1(new PlayerError(fail)))
      );
    }

    private async Task<OneOf<WriteError, PlayerError, GameStatus>> InvokeWonAsync(ProtectedJsonWriter writer,
      ProtectedPlayer player, bool won)
    {
      return await player.Won(won).Match(
        _ => writer.WriteAcknowledgeAsync().Match(
          _ => OneOf<WriteError, PlayerError, GameStatus>.FromT2(new GameStatus(Option<bool>.Some(won))),
          fail => OneOf<WriteError, PlayerError, GameStatus>.FromT0(new WriteError(fail))
        ),
        fail => Task.FromResult(OneOf<WriteError, PlayerError, GameStatus>.FromT1(new PlayerError(fail)))
      );
    }

    private sealed class GameStatus
    {
      public Option<bool> Value { get; }

      public GameStatus(Option<bool> value)
      {
        Value = value;
      }
    }

    private sealed class ProtectedJsonReader
    {
      private readonly JsonReader _reader;

      public ProtectedJsonReader(TextReader reader)
      {
        _reader = new JsonTextReader(new CharacterReader(reader)) {SupportMultipleContent = true};
      }

      public async Task<bool> ReadAsync()
      {
        TryAsync<bool> maybeRead = async () => await _reader.ReadAsync();
        return await maybeRead.Match(read => read, _ => false);
      }

      public Try<OneOf<SetupCall, TakeTurnCall, WonCall>> ReadMethodCall()
      {
        return () => CustomSerializer.Instance.Deserialize<OneOf<SetupCall, TakeTurnCall, WonCall>>(_reader)!;
      }
    }

    private sealed class ProtectedJsonWriter
    {
      private readonly TextWriter _writer;

      public ProtectedJsonWriter(TextWriter writer)
      {
        _writer = writer;
      }

      public TryAsync<Unit> WriteAcknowledgeAsync()
      {
        return async () =>
        {
          CustomSerializer.Instance.Serialize(_writer, Acknowledge.Value);
          await _writer.FlushAsync();
          return Unit.Default;
        };
      }

      public TryAsync<Unit> WriteMaybeMoveAsync(Either<IMove, Pass> maybeMove)
      {
        return async () =>
        {
          CustomSerializer.Instance.Serialize(_writer, maybeMove);
          await _writer.FlushAsync();
          return Unit.Default;
        };
      }
    }

    private sealed class ProtectedPlayer
    {
      private readonly IPlayer _player;

      public ProtectedPlayer(IPlayer player)
      {
        _player = player;
      }

      public Try<Acknowledge> Setup(Option<IPlayerState> initialState, BoardPosition goal) =>
        () => _player.Setup(initialState, goal);

      public Try<Either<IMove, Pass>> TakeTurn(IPlayerState state) => () => _player.TakeTurn(state);

      public Try<Acknowledge> Won(bool won) => () => _player.Won(won);
    }
  }
}