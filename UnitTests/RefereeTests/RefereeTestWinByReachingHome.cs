using System.Collections.Generic;
using Common;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests
{
  /// <summary>
  /// Tests the referee when a single player successfully reaches home after capturing their assigned treasure
  /// </summary>
  public class RefereeTestWinByReachingHome
  {
    [Fact]
    public void Test()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      var orangePlayer = new MockPlayer("orange", new PassStrategy());
      var pinkPlayer = new MockPlayer("pink", new EuclidStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWinByReachingHome();
      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);

      Assert.Single(result.winningPlayers);
      IPlayer winningPlayer = result.winningPlayers[0];
      Assert.Equal(pinkPlayer, winningPlayer);
      Assert.Empty(result.misbehavedPlayers);
      
      // Check purple player
      Assert.True(purplePlayer.CalledSetup);
      Assert.Equal(8, purplePlayer.NumberOfTurns);
      Assert.True(purplePlayer.CalledWon);
      Assert.False(purplePlayer.PlayerWon);
      
      // Check orange player
      Assert.True(orangePlayer.CalledSetup);
      Assert.Equal(8, orangePlayer.NumberOfTurns);
      Assert.True(orangePlayer.CalledWon);
      Assert.False(orangePlayer.PlayerWon);

      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(8, pinkPlayer.NumberOfTurns);
      Assert.True(pinkPlayer.CalledWon);
      Assert.True(pinkPlayer.PlayerWon);
    }
  }
}