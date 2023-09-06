using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents the strategy that determines the move as follows:
  /// 1. It uses the GoalStrategy to reach the given goal
  /// 2. If (1.) fails it chooses alternative goals with the smallest Euclidean distance to the original goal.
  ///    To break ties among positions that have the same distance to the original goal, it chooses the one with
  ///    the smaller row-column order.
  /// </summary>
  public sealed class EuclidStrategy : IPlayerStrategy
  {
    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      IPlayerStrategy internalStrategy = new AlternativeGoalStrategy(FindAlternativeGoals(state.Board, goal));
      return internalStrategy.ChooseMove(state, rule, goal);
    }

    private static IEnumerable<BoardPosition> FindAlternativeGoals(IImmutableBoard board, BoardPosition goal)
    {
      return board.Positions
        .OrderBy(goal.EuclidDistance)
        .ThenBy(position => position, new RowColumnComparer());
    }

    private sealed class RowColumnComparer : IComparer<BoardPosition>
    {
      public int Compare(BoardPosition position1, BoardPosition position2)
      {
        int rowDiff = position1.RowIndex - position2.RowIndex;
        if (rowDiff == 0)
        {
          return position1.ColumnIndex - position2.ColumnIndex;
        }

        return rowDiff;
      }
    }
  }
}