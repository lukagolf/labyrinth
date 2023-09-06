using Common;
using LanguageExt;
using Players;
using Xunit;
using static UnitTests.Players.StrategyTestUtilities;

namespace UnitTests.Players
{
  public class SlideRowRightStrategyTests
  { 
    [Fact]
    public void TestSuccess1()
    {
      var spareTile = new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(1, 2));
      var goal = new BoardPosition(5, 1);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideRowRight, 0));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowRight, 0), Rotation.OneHundredEighty, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestSuccess2()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(6, 6));
      var goal = new BoardPosition(0, 0);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideRowRight, 6));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideRowRight, 6), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }
    
    [Fact]
    public void TestFail1()
    {
      var spareTile = new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(1, 2));
      var goal = new BoardPosition(6, 0);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideRowRight, 0));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }

    [Fact]
    public void TestFail2()
    {
      var spareTile = new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(6, 6));
      var goal = new BoardPosition(2, 2);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideRowRight, 6));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }
  }
}