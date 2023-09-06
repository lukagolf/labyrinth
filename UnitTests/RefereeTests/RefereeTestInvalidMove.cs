using System.Collections.Generic;
using Common;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests
{
  /// <summary>
  /// Tests that the referee kicks out players who make an illegal move
  /// </summary>
  public class RefereeTestInvalidMove
  {
    [Fact]
    public void TestInvalidMove()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());

      var orangeMove = new Move(new SlideAction(SlideType.SlideColumnUp, 0), Rotation.OneHundredEighty,
        new BoardPosition(4, 3));
      var orangePlayer = new MockPlayer("orange", new PassThenMoveStrategy(orangeMove, 4));
      
      var pinkPlayer = new MockPlayer("pink", new EuclidStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWinByReachingHome();
      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);

      Assert.Single(result.winningPlayers);
      Assert.Equal(pinkPlayer, result.winningPlayers[0]);
      Assert.Single(result.misbehavedPlayers);
      Assert.Equal(orangePlayer, result.misbehavedPlayers[0]);
      
      // Check purple player
      Assert.True(purplePlayer.CalledSetup);
      Assert.Equal(8, purplePlayer.NumberOfTurns);
      Assert.True(purplePlayer.CalledWon);
      Assert.False(purplePlayer.PlayerWon);
      
      // Check orange player
      Assert.True(orangePlayer.CalledSetup);
      Assert.Equal(5, orangePlayer.NumberOfTurns);
      Assert.False(orangePlayer.CalledWon);
      Assert.False(orangePlayer.PlayerWon);
      
      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(8, pinkPlayer.NumberOfTurns);
      Assert.True(pinkPlayer.CalledWon);
      Assert.True(pinkPlayer.PlayerWon);
    }
  }
}