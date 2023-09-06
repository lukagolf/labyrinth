using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that uses a GoalStrategy to first try to reach the primary goal,
  /// and then tries to reach alternative goals in the order they are given. If it can't reach any goal,
  /// it produces a Pass.
  /// </summary>
  public class AlternativeGoalStrategy : IPlayerStrategy
  {
    private readonly IPlayerStrategy _internalStrategy;
    private readonly IList<BoardPosition> _alternativeGoals;

    public AlternativeGoalStrategy(IEnumerable<BoardPosition> goalList)
    {
      _internalStrategy = new GoalStrategy();
      _alternativeGoals = goalList.ToList();
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      return _internalStrategy.ChooseMove(state, rule, goal)
        .Some(Option<Either<IMove, Pass>>.Some)
        .None(() => ChooseAlternativeMove(state, rule));
    }

    private Option<Either<IMove, Pass>> ChooseAlternativeMove(IPlayerState state, IRule rule)
    {
      Option<BoardPosition> maybeGoal =
        _alternativeGoals.Find(goal => _internalStrategy.ChooseMove(state, rule, goal).IsSome);

      return maybeGoal
        .Some(goal => _internalStrategy.ChooseMove(state, rule, goal))
        .None(() => Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Right(Pass.Value)));
    }
  }
}