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
  /// Tests for multiple treasure chase games where there are multiple winners
  /// </summary>
  public class MultipleWinnerTests
  {
    // The game ends after 1000 rounds. The winners share the same minimum distance to their next "goal". First winner
    // has a minimum distance to its home. The second winner has a minimum distance to its next treasure.
    [Fact]
    public void Test1()
    {
      var treasures = new List<ITreasure> {new Treasure(Gem.AlexandritePearShape, Gem.Citrine)};

      var purplePlayer = new MockPlayer("purple", new PassStrategy());

      var move1 = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.TwoHundredSeventy,
        new BoardPosition(3, 2));
      var move2 = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy,
        new BoardPosition(4, 2));
      var orangePlayer = new MockPlayer("orange", new ToggleMoveStrategy(move1, move2));

      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(treasures);
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 2;
      purpleInfo.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(100);
      purpleInfo.CurrentPosition = new BoardPosition(2, 0);

      orangeInfo.NumberOfCapturedTreasures = 2;
      orangeInfo.CurrentPosition = new BoardPosition(4, 2);

      pinkInfo.NumberOfCapturedTreasures = 1;
      pinkInfo.CurrentPosition = new BoardPosition(5, 5);


      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Equal(2, result.winningPlayers.Count);
      Assert.Equal(purplePlayer, result.winningPlayers[0]);
      Assert.Equal(orangePlayer, result.winningPlayers[1]);
      Assert.Empty(result.misbehavedPlayers);
      Assert.Equal(1000, orangePlayer.NumberOfTurns);
    }

    // The game ends after 1000 rounds. The winners share the same minimum distance to their next "goal". First winner
    // has a minimum distance to its home. The second winner also has a minimum distance to its home.
    [Fact]
    public void Test2()
    {
      var treasures = new List<ITreasure> {new Treasure(Gem.AlexandritePearShape, Gem.Citrine)};

      var purplePlayer = new MockPlayer("purple", new PassStrategy());

      var move1 = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.TwoHundredSeventy,
        new BoardPosition(2, 3));
      var move2 = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy,
        new BoardPosition(2, 4));
      var orangePlayer = new MockPlayer("orange", new ToggleMoveStrategy(move1, move2));

      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(treasures);
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 2;
      purpleInfo.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(100);
      purpleInfo.CurrentPosition = new BoardPosition(2, 0);

      orangeInfo.NumberOfCapturedTreasures = 2;
      orangeInfo.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(100);
      orangeInfo.CurrentPosition = new BoardPosition(2, 4);

      pinkInfo.NumberOfCapturedTreasures = 1;
      pinkInfo.CurrentPosition = new BoardPosition(5, 5);


      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Equal(2, result.winningPlayers.Count);
      Assert.Equal(purplePlayer, result.winningPlayers[0]);
      Assert.Equal(orangePlayer, result.winningPlayers[1]);
      Assert.Empty(result.misbehavedPlayers);
      Assert.Equal(1000, orangePlayer.NumberOfTurns);
    }

    // The game ends after 1000 rounds. The winners share the same minimum distance to their next "goal". First winner
    // has a minimum distance to its next treasure. The second winner also has a minimum distance to its next treasure.
    [Fact]
    public void Test3()
    {
      var treasures = new List<ITreasure> {new Treasure(Gem.AlexandritePearShape, Gem.Citrine)};

      var purplePlayer = new MockPlayer("purple", new PassStrategy());

      var move1 = new Move(new SlideAction(SlideType.SlideRowLeft, 0), Rotation.TwoHundredSeventy,
        new BoardPosition(4, 5));
      var move2 = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy,
        new BoardPosition(4, 4));
      var orangePlayer = new MockPlayer("orange", new ToggleMoveStrategy(move1, move2));

      var pinkPlayer = new MockPlayer("pink", new PassStrategy());

      var players = new List<IPlayer> {purplePlayer, orangePlayer, pinkPlayer};
      IRefereeState state = CreateGameStateWithExtraTreasures(treasures);
      var purpleInfo = state.GetPlayer(Color.Purple);
      var orangeInfo = state.GetPlayer(Color.Orange);
      var pinkInfo = state.GetPlayer(Color.Pink);

      purpleInfo.NumberOfCapturedTreasures = 2;
      purpleInfo.CurrentPosition = new BoardPosition(4, 0);

      orangeInfo.NumberOfCapturedTreasures = 2;
      orangeInfo.CurrentPosition = new BoardPosition(4, 4);

      pinkInfo.NumberOfCapturedTreasures = 1;
      pinkInfo.CurrentPosition = new BoardPosition(5, 5);


      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      Assert.Equal(2, result.winningPlayers.Count);
      Assert.Equal(purplePlayer, result.winningPlayers[0]);
      Assert.Equal(orangePlayer, result.winningPlayers[1]);
      Assert.Empty(result.misbehavedPlayers);
      Assert.Equal(1000, orangePlayer.NumberOfTurns);
    }
  }
}