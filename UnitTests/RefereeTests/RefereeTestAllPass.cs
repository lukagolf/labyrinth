using System.Collections.Generic;
using Common;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests
{
  /// <summary>
  /// Tests this game over condition: All players take a pass
  ///
  /// Tests this winning condition:
  /// If no players are on their way back from their assigned goals, winners are all those players that share the
  /// same minimal Euclidean distance to their respective goal tile.
  /// </summary>
  public class RefereeTestAllPass
  {
    [Fact]
    public void Test()
    {
      var purpleMove = new Move(new SlideAction(SlideType.SlideRowRight, 0), Rotation.Ninety,
        new BoardPosition(1, 0));
      var purplePlayer = new MockPlayer("purple", new MoveAndThenPassStrategy(purpleMove));

      var orangeMove = new Move(new SlideAction(SlideType.SlideRowLeft, 6), Rotation.OneHundredEighty,
        new BoardPosition(2, 3));
      var orangePlayer = new MockPlayer("orange", new MoveAndThenPassStrategy(orangeMove));

      var pinkMove = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.TwoHundredSeventy,
        new BoardPosition(2, 5));
      var pinkPlayer = new MockPlayer("pink", new MoveAndThenPassStrategy(pinkMove));

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameState();
      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);

      Assert.Equal(2, result.winningPlayers.Count);
      Assert.Equal(orangePlayer, result.winningPlayers[0]);
      Assert.Equal(pinkPlayer, result.winningPlayers[1]);
      Assert.Empty(result.misbehavedPlayers);

      // Check purple player
      Assert.True(purplePlayer.CalledSetup);
      Assert.Equal(2, purplePlayer.NumberOfTurns);
      Assert.True(purplePlayer.CalledWon);
      Assert.False(purplePlayer.PlayerWon);

      // Check orange player
      Assert.True(orangePlayer.CalledSetup);
      Assert.Equal(2, orangePlayer.NumberOfTurns);
      Assert.True(orangePlayer.CalledWon);
      Assert.True(orangePlayer.PlayerWon);

      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(2, pinkPlayer.NumberOfTurns);
      Assert.True(pinkPlayer.CalledWon);
      Assert.True(pinkPlayer.PlayerWon);
    }
  }
}