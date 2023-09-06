using Common;
using LanguageExt;
using Players;
using Xunit;
using static UnitTests.Players.StrategyTestUtilities;

namespace UnitTests.Players
{
  public class RiemannStrategyTests
  {
    // test primary goal is reachable
    [Fact]
    public void TestSuccess1()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(3, 3));
      var goal = new BoardPosition(0, 5);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new RiemannStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnUp, 4), Rotation.Zero, goal);
      AssertMoveEquals(result, expectedMove);
    }
    
    // test primary goal is reachable
    [Fact]
    public void TestSuccess2()
    {
      var spareTile = new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(2, 0));
      var goal = new BoardPosition(5, 5);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new RiemannStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowLeft, 2), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }
    
    // test that Riemann chooses the first possible alternative position on the board according to row-column order
    [Fact]
    public void TestSuccess3()
    {
      var spareTile = new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(4, 6));
      var goal = new BoardPosition(3, 1);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new RiemannStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowRight, 4), Rotation.OneHundredEighty, new BoardPosition(0,0));
      AssertMoveEquals(result, expectedMove);
    }
    
    // test that Riemann chooses the first possible alternative position on the board according to row-column order
    [Fact]
    public void TestSuccess4()
    {
      var spareTile = new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(5, 3));
      var goal = new BoardPosition(1, 4);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new RiemannStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnUp, 2), Rotation.Zero, new BoardPosition(5,1));
      AssertMoveEquals(result, expectedMove);
    }

    // test that Riemann chooses the first possible alternative position on the board according to row-column order
    [Fact]
    public void TestSuccess5()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(2, 4));
      var goal = new BoardPosition(5, 3);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new RiemannStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowLeft, 2), Rotation.Zero, new BoardPosition(0,3));
      AssertMoveEquals(result, expectedMove);
    }
    
    [Fact]
    public void TestSuccess6()
    {
      var spareTile = new Tile(false, true, false, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(6, 2));
      var goal = new BoardPosition(0, 6);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new RiemannStrategy();
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnDown, 2), Rotation.Zero, new BoardPosition(0,2));
      AssertMoveEquals(result, expectedMove);
    }
  }
}