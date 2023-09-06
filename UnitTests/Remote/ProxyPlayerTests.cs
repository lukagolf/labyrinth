using System;
using System.IO;
using Common;
using JsonUtilities;
using LanguageExt;
using Newtonsoft.Json;
using Players;
using Remote;
using Xunit;
using static UnitTests.Utilities;


namespace UnitTests.Remote
{
  public class ProxyPlayerTests
  {
    [Fact]
    public void TestSetupNoneState()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, Acknowledge.Value);
      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      IPlayer player = new ProxyPlayer("Matthias", mockReader, mockWriter);
      Acknowledge ack = player.Setup(Option<IPlayerState>.None, new BoardPosition(5, 3));
      Assert.NotNull(ack);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      SetupCall setupCall = CustomSerializer.Instance.Deserialize<SetupCall>(mockWriterReader)!;

      Assert.True(setupCall.State.IsNone);
      Assert.Equal(new BoardPosition(5, 3), setupCall.Goal);
    }

    [Fact]
    public void TestSetupSomeState()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, Acknowledge.Value);
      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      IPlayer player = new ProxyPlayer("Matthias", mockReader, mockWriter);
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();
      Acknowledge ack = player.Setup(Option<IPlayerState>.Some(state), new BoardPosition(5, 3));
      Assert.NotNull(ack);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      SetupCall setupCall = CustomSerializer.Instance.Deserialize<SetupCall>(mockWriterReader)!;

      Assert.True(setupCall.State.IsSome);
      setupCall.State.IfSome(s => AssertSamePlayerState(state, s));
      Assert.Equal(new BoardPosition(5, 3), setupCall.Goal);
    }

    [Fact]
    public void TestTakeTurnPass()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, Either<IMove, Pass>.Right(Pass.Value));
      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      IPlayer player = new ProxyPlayer("Matthias", mockReader, mockWriter);
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();
      Either<IMove, Pass> maybeMove = player.TakeTurn(state);

      Assert.True(maybeMove.IsRight);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      TakeTurnCall takeTurnCall = CustomSerializer.Instance.Deserialize<TakeTurnCall>(mockWriterReader)!;

      AssertSamePlayerState(state, takeTurnCall.State);
    }

    [Fact]
    public void TestTakeTurnMove()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      IMove move = new Move(new SlideAction(SlideType.SlideColumnUp, 0), Rotation.OneHundredEighty,
        new BoardPosition(3, 2));
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, Either<IMove, Pass>.Left(move));
      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      IPlayer player = new ProxyPlayer("Matthias", mockReader, mockWriter);
      IRefereeStateBuilder stateBuilder = new RandomRefereeStateBuilder(new Random(33));
      IPlayerState state = stateBuilder.BuildState(9).ToPlayerState();
      Either<IMove, Pass> maybeMove = player.TakeTurn(state);

      Assert.True(maybeMove.Match(_ => false, m => m.Equals(move)));

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      TakeTurnCall takeTurnCall = CustomSerializer.Instance.Deserialize<TakeTurnCall>(mockWriterReader)!;

      AssertSamePlayerState(state, takeTurnCall.State);
    }

    [Fact]
    public void TestWonTrue()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, Acknowledge.Value);
      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      IPlayer player = new ProxyPlayer("Matthias", mockReader, mockWriter);
      Acknowledge ack = player.Won(true);
      Assert.NotNull(ack);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      WonCall wonCall = CustomSerializer.Instance.Deserialize<WonCall>(mockWriterReader)!;

      Assert.True(wonCall.Won);
    }

    [Fact]
    public void TestWonFalse()
    {
      var mockReaderContent = new StringWriter();
      var mockReaderContentWriter = new JsonTextWriter(mockReaderContent);
      CustomSerializer.Instance.Serialize(mockReaderContentWriter, Acknowledge.Value);
      var mockReader = new StringReader(mockReaderContent.ToString());
      var mockWriter = new StringWriter();

      IPlayer player = new ProxyPlayer("Matthias", mockReader, mockWriter);
      Acknowledge ack = player.Won(false);
      Assert.NotNull(ack);

      var mockWriterReader = new JsonTextReader(new StringReader(mockWriter.ToString()))
        {SupportMultipleContent = true};
      WonCall wonCall = CustomSerializer.Instance.Deserialize<WonCall>(mockWriterReader)!;

      Assert.False(wonCall.Won);
    }
  }
}