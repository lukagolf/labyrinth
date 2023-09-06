using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Common;
using JsonUtilities;
using LanguageExt;
using Newtonsoft.Json;
using Players;
using Remote;
using UnitTests.RefereeTests;
using Xunit;

namespace UnitTests.Remote
{
  public class PlayerDispatcherTests
  {
    [Fact]
    public async Task TestReadError()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      var setupCall = new SetupCall(Option<IPlayerState>.None, new BoardPosition(2, 5));
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();
      var takeTurnCall = new TakeTurnCall(state);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, setupCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);

      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      MockPlayer mockPlayer = new MockPlayer("Mock", new PassStrategy());
      IPlayerDispatcher dispatcher = new PlayerDispatcher();
      Result result = await dispatcher.RunAsync(mockReader, mockWriter, mockPlayer);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      await mockWriterReader.ReadAsync();
      Acknowledge ack = CustomSerializer.Instance.Deserialize<Acknowledge>(mockWriterReader)!;
      Assert.NotNull(ack);

      await mockWriterReader.ReadAsync();
      Either<IMove, Pass> maybeMove = CustomSerializer.Instance.Deserialize<Either<IMove, Pass>>(mockWriterReader)!;
      Assert.True(maybeMove.IsRight);

      Assert.True(result.Value.IsT0);
      Assert.True(mockPlayer.CalledSetup);
      Assert.True(mockPlayer.CalledTakeTurn);
      Assert.False(mockPlayer.CalledWon);
      Assert.False(mockPlayer.PlayerWon);
    }

    [Fact]
    public async Task TestWriteError()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      var setupCall = new SetupCall(Option<IPlayerState>.None, new BoardPosition(2, 5));
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();
      var takeTurnCall = new TakeTurnCall(state);
      var wonCall = new WonCall(true);

      CustomSerializer.Instance.Serialize(mockReaderContentWriter, setupCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, wonCall);

      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new MockWriter();

      MockPlayer mockPlayer = new MockPlayer("Mock", new PassStrategy());
      IPlayerDispatcher dispatcher = new PlayerDispatcher();
      Result result = await dispatcher.RunAsync(mockReader, mockWriter, mockPlayer);

      Assert.True(result.Value.IsT1);
      Assert.Equal("Mock write exception", result.Value.AsT1.Exception.Message);
      Assert.True(mockPlayer.CalledSetup);
      Assert.True(mockPlayer.CalledTakeTurn);
      Assert.True(mockPlayer.CalledWon);
      Assert.True(mockPlayer.PlayerWon);
    }

    [Fact]
    public async Task TestPlayerError()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      var setupCall = new SetupCall(Option<IPlayerState>.None, new BoardPosition(2, 5));
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();
      var takeTurnCall = new TakeTurnCall(state);
      var wonCall = new WonCall(true);

      CustomSerializer.Instance.Serialize(mockReaderContentWriter, setupCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, wonCall);

      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      var mockPlayer = new MockPlayer("Mock", new ThrowExceptionStrategy(2));
      IPlayerDispatcher dispatcher = new PlayerDispatcher();
      Result result = await dispatcher.RunAsync(mockReader, mockWriter, mockPlayer);

      Assert.True(result.Value.IsT2);
      Assert.Equal("Mock strategy exception", result.Value.AsT2.Exception.Message);
      Assert.True(mockPlayer.CalledSetup);
      Assert.True(mockPlayer.CalledTakeTurn);
      Assert.False(mockPlayer.CalledWon);
      Assert.False(mockPlayer.PlayerWon);
    }

    [Fact]
    public async Task TestLose()
    {
      await TestGameEnd(false);
    }

    [Fact]
    public async Task TestWin()
    {
      await TestGameEnd(true);
    }

    private static async Task TestGameEnd(bool won)
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();

      var setupCall = new SetupCall(Option<IPlayerState>.Some(state), new BoardPosition(2, 5));
      var takeTurnCall = new TakeTurnCall(state);
      var wonCall = new WonCall(won);

      CustomSerializer.Instance.Serialize(mockReaderContentWriter, setupCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, takeTurnCall);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, wonCall);

      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      var move1 = new Move(new SlideAction(SlideType.SlideRowRight, 0), Rotation.Ninety, new BoardPosition(4, 3));
      var move2 = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.Zero, new BoardPosition(4, 3));
      var mockPlayer = new MockPlayer("Mock", new ToggleMoveStrategy(move1, move2));
      IPlayerDispatcher dispatcher = new PlayerDispatcher();
      Result result = await dispatcher.RunAsync(mockReader, mockWriter, mockPlayer);

      Assert.True(result.Value.IsT3);
      Assert.Equal(won, result.Value.AsT3);
      Assert.True(mockPlayer.CalledSetup);
      Assert.True(mockPlayer.CalledTakeTurn);
      Assert.True(mockPlayer.CalledWon);
      Assert.Equal(won, mockPlayer.PlayerWon);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      await mockWriterReader.ReadAsync();
      Acknowledge ack = CustomSerializer.Instance.Deserialize<Acknowledge>(mockWriterReader)!;
      Assert.NotNull(ack);

      await mockWriterReader.ReadAsync();
      Either<IMove, Pass> maybeMove = CustomSerializer.Instance.Deserialize<Either<IMove, Pass>>(mockWriterReader)!;
      Assert.True(maybeMove.IsLeft);
      Assert.True(maybeMove.Match(_ => false, move => move.Equals(move1)));

      await mockWriterReader.ReadAsync();
      maybeMove = CustomSerializer.Instance.Deserialize<Either<IMove, Pass>>(mockWriterReader)!;
      Assert.True(maybeMove.IsLeft);
      Assert.True(maybeMove.Match(_ => false, move => move.Equals(move2)));

      await mockWriterReader.ReadAsync();
      ack = CustomSerializer.Instance.Deserialize<Acknowledge>(mockWriterReader)!;
      Assert.NotNull(ack);
    }

    private sealed class MockWriter : StringWriter
    {
      private int _counter;

      public MockWriter()
      {
        _counter = 0;
      }

      public override Encoding Encoding => Encoding.Default;

      public override void Write(string? value)
      {
        _counter += 1;
        if (_counter >= 3)
        {
          throw new Exception("Mock write exception");
        }

        base.Write(value);
      }
    }
  }
}