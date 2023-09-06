using Common;
using LanguageExt;
using Players;
using Xunit;
using static UnitTests.Players.StrategyTestUtilities;

namespace UnitTests.Players
{
  public class SlideColumnDownStrategyTests
  {
    [Fact]
    public void TestSuccess1()
    {
      var spareTile = new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 5));
      var goal = new BoardPosition(2, 6);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnDown, 6));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnDown, 6), Rotation.Ninety, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestFail1()
    {
      var spareTile = new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 5));
      var goal = new BoardPosition(3, 6);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnDown, 6));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }

    [Fact]
    public void TestSuccess2()
    {
      var spareTile = new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 5));
      var goal = new BoardPosition(3, 4);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnDown, 4));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      IMove expectedMove = new Move(new SlideAction(SlideType.SlideColumnDown, 4), Rotation.TwoHundredSeventy, goal);
      AssertMoveEquals(result, expectedMove);
    }

    [Fact]
    public void TestFail2()
    {
      var spareTile = new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Aplite));
      IPlayerState state = CreateGameState(spareTile, new BoardPosition(0, 5));
      var goal = new BoardPosition(4, 4);
      IRule rule = new ReachableRule();

      IPlayerStrategy strategy = new SlideStrategy(new SlideAction(SlideType.SlideColumnDown, 4));
      Option<Either<IMove, Pass>> result = strategy.ChooseMove(state, rule, goal);
      Assert.True(result.IsNone);
    }
  }
}