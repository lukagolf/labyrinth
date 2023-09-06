using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that tries to reach a given goal by trying to perform every possible row slide
  /// (first left, then right) from top to down. If it doesn't reach the goal it performs every possible column
  /// slide (first up, then down) from left to right.
  /// </summary>
  public sealed class GoalStrategy : IPlayerStrategy
  {
    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      IPlayerStrategy internalStrategy = CreateInternalStrategy(state.Board);
      return internalStrategy.ChooseMove(state, rule, goal);
    }

    private IPlayerStrategy CreateInternalStrategy(IImmutableBoard board)
    {
      IEnumerable<IPlayerStrategy> slideRowStrategies = board.SlidableRows
        .Select(rowIdx => new SlideRowStrategy(rowIdx));

      IEnumerable<IPlayerStrategy> slideColumnStrategies = board.SlidableColumns
        .Select(rowIdx => new SlideColumnStrategy(rowIdx));

      return new TryManyStrategy(
        slideRowStrategies.Concat(slideColumnStrategies)
      );
    }
  }
}