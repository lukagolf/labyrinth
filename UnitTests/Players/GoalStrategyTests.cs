using Common;
using LanguageExt;
using Players;
using Xunit;
using static UnitTests.Players.StrategyTestUtilities;

namespace UnitTests.Players
{
  public class GoalStrategyTests
  {
    [Fact]
    public void TestSuccess1()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(3, 3));
      var goal = new BoardPosition(0, 5);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new GoalStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnUp, 4), Rotation.Zero, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestSuccess2()
    {
      var spareTile = new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 5));
      var goal = new BoardPosition(3, 4);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new GoalStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnDown, 4), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestSuccess3()
    {
      var spareTile = new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(2, 0));
      var goal = new BoardPosition(5, 5);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new GoalStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowLeft, 2), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestSuccess4()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(6, 6));
      var goal = new BoardPosition(0, 0);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new GoalStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestFail1()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(4, 6));
      var goal = new BoardPosition(3, 1);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new GoalStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }

    [Fact]
    public void TestFail2()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(3, 1));
      var goal = new BoardPosition(4, 6);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new GoalStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }
  }
}