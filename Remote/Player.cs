using System;
using System.IO;
using Common;
using JsonUtilities;
using LanguageExt;
using Newtonsoft.Json;
using Players;

namespace Remote
{
  /// <summary>
  /// Provides a player implementation that allows the referee to communicate with remote players over a JSON protocol
  /// </summary>
  public sealed class ProxyPlayer : IPlayer
  {
    /// <summary>
    /// JSON reader that is used to receive method return values from the remote player
    /// </summary>
    private readonly JsonReader _reader;

    /// <summary>
    /// JSON writer that is used to send method requests to the remote player
    /// </summary>
    private readonly TextWriter _writer;

    public ProxyPlayer(string name, TextReader reader, TextWriter writer)
    {
      Name = name;
      _reader = new JsonTextReader(new CharacterReader(reader)) {SupportMultipleContent = true};
      _writer = writer;
    }

    public string Name { get; }

    public IBoard ProposeBoard(int rows, int columns)
    {
      throw new NotImplementedException();
    }

    public Acknowledge Setup(Option<IPlayerState> initialState, BoardPosition goal)
    {
      var setupCall = new SetupCall(initialState, goal);
      CustomSerializer.Instance.Serialize(_writer, setupCall);
      _writer.Flush();
      _reader.Read();
      return CustomSerializer.Instance.Deserialize<Acknowledge>(_reader)!;
    }

    public Either<IMove, Pass> TakeTurn(IPlayerState state)
    {
      var takeTurnCall = new TakeTurnCall(state);
      CustomSerializer.Instance.Serialize(_writer, takeTurnCall);
      _writer.Flush();
      _reader.Read();
      return CustomSerializer.Instance.Deserialize<Either<IMove, Pass>>(_reader)!;
    }

    public Acknowledge Won(bool won)
    {
      var wonCall = new WonCall(won);
      CustomSerializer.Instance.Serialize(_writer, wonCall);
      _writer.Flush();
      _reader.Read();
      return CustomSerializer.Instance.Deserialize<Acknowledge>(_reader)!;
    }
  }
}