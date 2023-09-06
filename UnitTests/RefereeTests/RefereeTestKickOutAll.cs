using System.Collections.Generic;
using Common;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests
{
  public class RefereeTestKickOutAll
  {
    [Fact]
    public void Test()
    {
      var purpleMove1 = new Move(new SlideAction(SlideType.SlideRowLeft, 4), Rotation.TwoHundredSeventy,
        new BoardPosition(0, 1));
      var purpleMove2 = new Move(new SlideAction(SlideType.SlideRowRight, 4), Rotation.TwoHundredSeventy,
        new BoardPosition(0, 0));
      var purpleToggleStrategy = new ToggleMoveStrategy(purpleMove1, purpleMove2);
      var purplePlayer = new MockPlayer("purple", new TakeTooLongStrategy(purpleToggleStrategy, 300));

      var orangeMove1 = new Move(new SlideAction(SlideType.SlideRowLeft, 6), Rotation.Ninety,
        new BoardPosition(1, 3));
      var orangeMove2 = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.Ninety,
        new BoardPosition(2, 3));
      var orangeToggleStrategy = new ToggleMoveStrategy(orangeMove1, orangeMove2);
      var orangePlayer = new MockPlayer("orange", new TakeTooLongStrategy(orangeToggleStrategy, 400));

      var pinkMove1 = new Move(new SlideAction(SlideType.SlideColumnUp, 6), Rotation.OneHundredEighty,
        new BoardPosition(3, 5));
      var pinkMove2 = new Move(new SlideAction(SlideType.SlideColumnDown, 2), Rotation.OneHundredEighty,
        new BoardPosition(2, 5));
      var pinkToggleStrategy = new ToggleMoveStrategy(pinkMove1, pinkMove2);
      var pinkPlayer = new MockPlayer("pink", new TakeTooLongStrategy(pinkToggleStrategy, 500));

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameState();


      state.GetPlayer(Color.Purple).CurrentPosition = new BoardPosition(0, 0);

      state.GetPlayer(Color.Orange).CurrentPosition = new BoardPosition(2, 3);

      state.GetPlayer(Color.Pink).CurrentPosition = new BoardPosition(2, 5);

      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);

      Assert.Empty(result.winningPlayers);
      Assert.Equal(3, result.misbehavedPlayers.Count);
      Assert.Equal(purplePlayer, result.misbehavedPlayers[0]);
      Assert.Equal(orangePlayer, result.misbehavedPlayers[1]);
      Assert.Equal(pinkPlayer, result.misbehavedPlayers[2]);

      // Check purple player
      Assert.True(purplePlayer.CalledSetup);
      Assert.Equal(301, purplePlayer.NumberOfTurns);
      Assert.False(purplePlayer.CalledWon);
      Assert.False(purplePlayer.PlayerWon);

      // Check orange player
      Assert.True(orangePlayer.CalledSetup);
      Assert.Equal(401, orangePlayer.NumberOfTurns);
      Assert.False(orangePlayer.CalledWon);
      Assert.False(orangePlayer.PlayerWon);

      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(501, pinkPlayer.NumberOfTurns);
      Assert.False(pinkPlayer.CalledWon);
      Assert.False(pinkPlayer.PlayerWon);
    }
  }
}