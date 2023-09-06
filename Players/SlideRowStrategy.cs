using System.Collections.Generic;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that tries to reach a given goal by sliding the row at the given index first to the
  /// left and if it fails to the right. Both sliding actions are performed by trying all four rotations of the
  /// spare tile.
  /// </summary>
  public sealed class SlideRowStrategy : IPlayerStrategy
  {
    /// <summary>
    /// Represents the internal strategy that this strategy will delegate to
    /// </summary>
    private readonly IPlayerStrategy _internalStrategy;

    /// <summary>
    /// Constructs a strategy that tries to reach a given goal by sliding the row at the given index first to the
    /// left and if it fails to the right.
    /// </summary>
    /// <param name="rowIdx">Index at which the row should be slided</param>
    public SlideRowStrategy(int rowIdx)
    {
      _internalStrategy = new TryManyStrategy(
        new List<IPlayerStrategy>
        {
          new SlideStrategy(new SlideAction(SlideType.SlideRowLeft, rowIdx)),
          new SlideStrategy(new SlideAction(SlideType.SlideRowRight, rowIdx)),
        }
      );
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      return _internalStrategy.ChooseMove(state, rule, goal);
    }
  }
}