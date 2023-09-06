using System.Collections.Generic;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that tries to reach a given goal by sliding the column at the given index first to
  /// up and if it fails to down. Both sliding actions are performed by trying all four rotations of the
  /// spare tile.
  /// </summary>
  public sealed class SlideColumnStrategy : IPlayerStrategy
  {
    /// <summary>
    /// Represents the internal strategy that this strategy will delegate to
    /// </summary>
    private readonly IPlayerStrategy _internalStrategy;

    /// <summary>
    /// Constructs a strategy that tries to reach a given goal by sliding the column at the given index first to
    /// up and if it fails to down.
    /// </summary>
    /// <param name="columnIdx">Index at which the column should be slided</param>
    public SlideColumnStrategy(int columnIdx)
    {
      _internalStrategy = new TryManyStrategy(
        new List<IPlayerStrategy>
        {
          new SlideStrategy(new SlideAction(SlideType.SlideColumnUp, columnIdx)),
          new SlideStrategy(new SlideAction(SlideType.SlideColumnDown, columnIdx)),
        }
      );
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      return _internalStrategy.ChooseMove(state, rule, goal);
    }
  }
}