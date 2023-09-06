using System.Collections.Generic;
using Common;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests
{
  /// <summary>
  /// The referee has run 1000 rounds 
  ///
  /// Tests this winning condition:
  /// Winners are all those players that are on their way back home and share the same minimal Euclidean distance
  /// to their home
  /// </summary>
  public class RefereeTestThousand
  {
    [Fact]
    public void Test()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      
      var orangePlayer = new MockPlayer("orange", new PassStrategy());

      var move1 = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.TwoHundredSeventy,
        new BoardPosition(3, 5));
      var move2 = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy,
        new BoardPosition(2, 5));
      var pinkPlayer = new MockPlayer("pink", new ToggleMoveStrategy(move1, move2));

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameState();

      state.GetPlayer(Color.Purple).CurrentPosition = new BoardPosition(0, 0);
      
      state.GetPlayer(Color.Orange).CurrentPosition = new BoardPosition(2, 3);
      
      state.GetPlayer(Color.Pink).CurrentPosition = new BoardPosition(2, 5);
        
      IReferee referee = new Referee.Referee(new RuleBook(), 100);
      var result = referee.RunGame(state, players);

      Assert.Equal(2, result.winningPlayers.Count);
      Assert.Equal(orangePlayer, result.winningPlayers[0]);
      Assert.Equal(pinkPlayer, result.winningPlayers[1]);
      Assert.Empty(result.misbehavedPlayers);
      
      // Check purple player
      Assert.True(purplePlayer.CalledSetup);
      Assert.Equal(1000, purplePlayer.NumberOfTurns);
      Assert.True(purplePlayer.CalledWon);
      Assert.False(purplePlayer.PlayerWon);
      
      // Check orange player
      Assert.True(orangePlayer.CalledSetup);
      Assert.Equal(1000, orangePlayer.NumberOfTurns);
      Assert.True(orangePlayer.CalledWon);
      Assert.True(orangePlayer.PlayerWon);
      
      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(1000, pinkPlayer.NumberOfTurns);
      Assert.True(pinkPlayer.CalledWon);
      Assert.True(pinkPlayer.PlayerWon);
    }
  }
}