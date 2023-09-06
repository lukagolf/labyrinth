using System;
using System.Collections.Generic;
using Common;
using LanguageExt;
using Players;
using Referee;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.RefereeTests.MultipleTreasureTests
{
  /// <summary>
  /// Tests for multiple treasure chase games where there is only a single winner
  /// </summary>
  public sealed class SingleWinnerTests
  {
    // Tests that the player who captures the maximum number of treasures is the winner
    [Fact]
    public void Test1()
    {
      var treasures = new List<ITreasure>
      {
        new Treasure(Gem.AlexandritePearShape, Gem.Aquamarine),
        new Treasure(Gem.AlexandritePearShape, Gem.Aventurine),
        new Treasure(Gem.AlexandritePearShape, Gem.Beryl)
      };
      
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      var orangePlayer = new MockPlayer("orange", new EuclidStrategy());
      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(treasures);
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 1;
      orangeInfo.NumberOfCapturedTreasures = 1;
      pinkInfo.NumberOfCapturedTreasures = 1;

      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Equal(1, purpleInfo.NumberOfCapturedTreasures);
      Assert.Equal(5, orangeInfo.NumberOfCapturedTreasures);
      Assert.Equal(1, pinkInfo.NumberOfCapturedTreasures);

      Assert.Equal(0, state.NumberOfTreasures);

      Assert.Single(result.winningPlayers);
      IPlayer winningPlayer = result.winningPlayers[0];
      Assert.Equal(orangePlayer, winningPlayer);
      Assert.Empty(result.misbehavedPlayers);
    }

    // Test that the winner is the one with the minimum distance to their next "goal" among players who captured the
    // same maximum number of treasures. The winner ends the game by reaching home, so it has a distance of 0.
    [Fact]
    public void Test2()
    {
      var treasures = new List<ITreasure>
      {
        new Treasure(Gem.AlexandritePearShape, Gem.Aquamarine)
      };
      
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      var orangePlayer = new MockPlayer("orange", new EuclidStrategy());
      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(treasures);
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 2;
      pinkInfo.NumberOfCapturedTreasures = 2;
      
      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Equal(2, purpleInfo.NumberOfCapturedTreasures);
      Assert.Equal(2, orangeInfo.NumberOfCapturedTreasures);
      Assert.Equal(2, pinkInfo.NumberOfCapturedTreasures);

      Assert.Equal(0, state.NumberOfTreasures);

      Assert.Single(result.winningPlayers);
      IPlayer winningPlayer = result.winningPlayers[0];
      Assert.Equal(orangePlayer, winningPlayer);
      Assert.Empty(result.misbehavedPlayers);
    }

    // Test that the winner is the one with the minimum distance to their next "goal" among players who captured the
    // same maximum number of treasures. The game ends after 1000 rounds. All players captured the same
    // number of treasures. The winner's distance to its next treasure is less than the distances other players have
    // to their homes
    [Fact]
    public void Test3()
    {
      var purplePlayer = new MockPlayer("purple", new PassStrategy());

      var move1 = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.TwoHundredSeventy,
        new BoardPosition(3, 2));
      var move2 = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy,
        new BoardPosition(4, 2));
      var orangePlayer = new MockPlayer("orange", new ToggleMoveStrategy(move1, move2));

      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(Array.Empty<ITreasure>());
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 2;
      purpleInfo.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(100);
      purpleInfo.CurrentPosition = new BoardPosition(5, 1);

      orangeInfo.NumberOfCapturedTreasures = 2;
      orangeInfo.CurrentPosition = new BoardPosition(4, 2);

      pinkInfo.NumberOfCapturedTreasures = 2;
      pinkInfo.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(100);
      pinkInfo.CurrentPosition = new BoardPosition(5, 5);

      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Single(result.winningPlayers);
      IPlayer winningPlayer = result.winningPlayers[0];
      Assert.Equal(orangePlayer, winningPlayer);
      Assert.Empty(result.misbehavedPlayers);
      Assert.Equal(1000, orangePlayer.NumberOfTurns);
    }
    
    /*
     * Tests that the player who captures the maximum number of treasures is the winner, with the following edge case:
     * What if the last treasure-goal in the extra-goals sequence coincides with the home of the player that is told
     * to go there? Too bad for the player. The goal gets removed from the sequence; it is assigned to the player;
     * but it doesn't count for the player as having collected another treasure.
     */
    [Fact]
    public void Test4()
    {
      var treasures = new List<ITreasure>
      {
        new Treasure(Gem.AlexandritePearShape, Gem.Aquamarine),
        new Treasure(Gem.AlexandritePearShape, Gem.Aventurine),
      };
      
      var purplePlayer = new MockPlayer("purple", new PassStrategy());
      var orangePlayer = new MockPlayer("orange", new EuclidStrategy());
      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(treasures);
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 1;
      orangeInfo.NumberOfCapturedTreasures = 1;
      pinkInfo.NumberOfCapturedTreasures = 1;

      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Equal(1, purpleInfo.NumberOfCapturedTreasures);
      // without the edge case above, this would be 4
      Assert.Equal(3, orangeInfo.NumberOfCapturedTreasures);
      Assert.Equal(1, pinkInfo.NumberOfCapturedTreasures);

      Assert.Equal(0, state.NumberOfTreasures);
      Assert.Single(result.winningPlayers);
      IPlayer winningPlayer = result.winningPlayers[0];
      Assert.Equal(orangePlayer, winningPlayer);
    }
  }
}