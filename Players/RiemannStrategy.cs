using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents the strategy that determines the move as follows:
  /// 1. It uses the GoalStrategy to reach the given goal
  /// 2. If (1.) fails it chooses an alternative goal starting from the top-most left corner of the board and proceeds
  ///    in the row-column order by trying to reach these alternative goals using the GoalStrategy.
  /// </summary>
  public sealed class RiemannStrategy : IPlayerStrategy
  {
    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      IPlayerStrategy internalStrategy = new AlternativeGoalStrategy(state.Board.Positions);
      return internalStrategy.ChooseMove(state, rule, goal);
    }
  }
}