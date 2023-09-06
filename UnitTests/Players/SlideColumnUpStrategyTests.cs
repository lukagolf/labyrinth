using Common;
using LanguageExt;
using Players;
using Xunit;
using static UnitTests.Players.StrategyTestUtilities;

namespace UnitTests.Players
{
  public class SlideColumnUpStrategyTests
  {
    [Fact]
    public void TestSuccess1()
    {
      var spareTile = new Tile(true, true, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(5, 0));
      var goal = new BoardPosition(0, 0);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnUp, 0));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnUp, 0), Rotation.Zero, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestFail1()
    {
      var spareTile = new Tile(true, true, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(5, 0));
      var goal = new BoardPosition(2, 2);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnUp, 0));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }

    [Fact]
    public void TestSuccess2()
    {
      var spareTile = new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 2));
      var goal = new BoardPosition(6, 3);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnUp, 2));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnUp, 2), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }
    
    [Fact]
    public void TestFail2()
    {
      var spareTile = new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 2));
      var goal = new BoardPosition(3, 5);
      IRule rule = new ReachableRule();
      
      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnUp, 2));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }
  }
}