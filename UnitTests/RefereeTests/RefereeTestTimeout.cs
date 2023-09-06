using System.Collections.Generic;
using Common;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests
{
  /// <summary>
  /// Tests that the referee kicks out players who take too long to respond
  /// </summary>
  public class RefereeTestTimeout
  {
    /// <summary>
    /// Tests when a player takes too long to respond to setup
    /// </summary>
    [Fact]
    public void TestSetupTimeout()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      
      var orangePlayer = new MockPlayer("orange", new PassStrategy())
      {
        TakeTooLongInSetup = true
      };

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
      Assert.Equal(0, orangePlayer.NumberOfTurns);
      Assert.False(orangePlayer.CalledWon);
      Assert.False(orangePlayer.PlayerWon);
      
      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(8, pinkPlayer.NumberOfTurns);
      Assert.True(pinkPlayer.CalledWon);
      Assert.True(pinkPlayer.PlayerWon);
    }
    
    /// <summary>
    /// Tests when a player takes too long to respond to take turn 
    /// </summary>
    [Fact]
    public void TestTakeTurnTimeout()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      var orangePlayer = new MockPlayer("orange", new TakeTooLongStrategy(new PassStrategy(), 5));
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
      Assert.Equal(6, orangePlayer.NumberOfTurns);
      Assert.False(orangePlayer.CalledWon);
      Assert.False(orangePlayer.PlayerWon);
      
      // Check pink player
      Assert.True(pinkPlayer.CalledSetup);
      Assert.Equal(8, pinkPlayer.NumberOfTurns);
      Assert.True(pinkPlayer.CalledWon);
      Assert.True(pinkPlayer.PlayerWon);
    }
    
    /// <summary>
    /// Test that the referee kicks out players who take too long in win request
    /// </summary>
    [Fact]
    public void TestTakeTooLongInWin()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      var orangePlayer = new MockPlayer("orange", new PassStrategy())
      {
        TakeTooLongInWon = true,
      };
      var pinkPlayer = new MockPlayer("pink", new EuclidStrategy())
      {
        TakeTooLongInWon = true,
      };

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWinByReachingHome();
      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);

      Assert.Empty(result.winningPlayers);
      Assert.Equal(2, result.misbehavedPlayers.Count);
      Assert.Equal(orangePlayer, result.misbehavedPlayers[0]);
      Assert.Equal(pinkPlayer, result.misbehavedPlayers[1]);
      
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