using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that tries to reach a given goal by executing the given sliding action with all
  /// four possible rotations of the spare tile.
  /// </summary>
  public sealed class SlideStrategy : IPlayerStrategy
  {
    /// <summary>
    /// Represents the sliding action this strategy will perform to reach the goal position
    /// </summary>
    private readonly ISlideAction _slideAction;

    /// <summary>
    /// Constructs a strategy that tries to reach the given goal by by executing the given sliding action.
    /// </summary>
    /// <param name="slideAction">The slide action to execute</param>
    public SlideStrategy(ISlideAction slideAction)
    {
      _slideAction = slideAction;
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      // Clockwise equivalent of the counter-clockwise rotation order given in the spec
      var rotations = new List<Rotation>
        {Rotation.Zero, Rotation.TwoHundredSeventy, Rotation.OneHundredEighty, Rotation.Ninety};

      IEnumerable<IMove> moves = rotations.Select(rotation =>
      {
        IMove move = new Move(_slideAction, rotation, goal);
        return move;
      });

      Option<IMove> maybeMove = moves.Find(move => rule.ValidateMove(state, move).IsLeft);
      return maybeMove
        .Some(move => Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Left(move)))
        .None(() => Option<Either<IMove, Pass>>.None);
    }
  }
}