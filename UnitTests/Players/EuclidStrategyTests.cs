using System.Collections.Generic;
using Common;
using LanguageExt;
using Players;
using Xunit;
using static UnitTests.Players.StrategyTestUtilities;

namespace UnitTests.Players
{
  public class EuclidStrategyTests
  {
    // test primary goal is reachable
    [Fact]
    public void TestSuccess1()
    {
      var spareTile = new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 5));
      var goal = new BoardPosition(3, 4);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnDown, 4), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }

    // test primary goal is reachable
    [Fact]
    public void TestSuccess2()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(6, 6));
      var goal = new BoardPosition(0, 0);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }

    // test that Euclid chooses the alternative position that is closest to the original goal
    [Fact]
    public void TestSuccess3()
    {
      var spareTile = new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 0));
      var goal = new BoardPosition(6, 6);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnDown, 2), Rotation.Zero,
        new BoardPosition(6, 2));
      AssertMoveEquals(result, expectedMove);
    }

    // test that Euclid chooses the alternative position that is closest to the original goal
    [Fact]
    public void TestSuccess4()
    {
      var spareTile = new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 6));
      var goal = new BoardPosition(4, 5);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnUp, 6), Rotation.OneHundredEighty,
        new BoardPosition(6, 5));
      AssertMoveEquals(result, expectedMove);
    }

    // test that Euclid chooses the alternative position that is closest to the original goal
    [Fact]
    public void TestSuccess5()
    {
      var spareTile = new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(4, 3));
      var goal = new BoardPosition(1, 2);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove =
        new Move(new SlideAction(SlideType.SlideRowRight, 4), Rotation.Zero, new BoardPosition(1, 3));
      AssertMoveEquals(result, expectedMove);
    }

    // test that Euclid chooses the alternative position that is closest to the original goal                                                                               
    [Fact]
    public void TestSuccess6()
    {
      var spareTile = new Tile(true, true, false, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(2, 6));
      var goal = new BoardPosition(6, 3);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowLeft, 2), Rotation.Zero, new BoardPosition(6, 4));
      AssertMoveEquals(result, expectedMove);
    }

    // test that Euclid prefers the tile that has lower row column order when it must choose between multiple
    // tiles that have the same distance to the goal tile
    [Fact]
    public void TestSuccess7()
    {
      IPlayerState state = CreateGameStateRowColumnOrder(new BoardPosition(1, 2));
      var goal = new BoardPosition(1, 1);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new EuclidStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowLeft, 2), Rotation.Zero, new BoardPosition(0, 1));
      AssertMoveEquals(result, expectedMove);
    }

    // Creates a new game state with a board that is specifically constructed to test that Euclid prefers
    // the tile that has lower row column order when it must choose between multiple tiles that have the same
    // distance to the goal tile
    private static IPlayerState CreateGameStateRowColumnOrder(BoardPosition playerPosition)
    {
      var spareTile = new Tile(true, true, true, true, new Treasure(Gem.Diamond, Gem.Prehnite));
      var tiles = new ITile[,]
      {
        {
          new Tile(false, false, true, true, new Treasure(Gem.Diamond, Gem.Prehnite)),
          new Tile(false, true, false, true, new Treasure(Gem.Diamond, Gem.Prehnite)),
          new Tile(false, true, true, false, new Treasure(Gem.Diamond, Gem.Prehnite)),
        },
        {
          new Tile(true, false, true, false, new Treasure(Gem.Diamond, Gem.Prehnite)),
          new Tile(true, false, true, false, new Treasure(Gem.Diamond, Gem.Prehnite)),
          new Tile(true, false, true, false, new Treasure(Gem.Diamond, Gem.Prehnite)),
        },
        {
          new Tile(false, true, true, false, new Treasure(Gem.Diamond, Gem.Prehnite)),
          new Tile(false, true, true, false, new Treasure(Gem.Diamond, Gem.Prehnite)),
          new Tile(false, false, true, true, new Treasure(Gem.Diamond, Gem.Prehnite)),
        },
      };

      IBoard board = new Board(tiles);

      var player = new PublicPlayerInfo(Color.Purple, new BoardPosition(1, 1), playerPosition);
      var players = new List<IPublicPlayerInfo> {player};
      return new PlayerState(players, board, spareTile);
    }
  }
}