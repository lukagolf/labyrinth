using Common;
using LanguageExt;

namespace Players
{
  public class ReachableRule : IRule
  {
    public Either<ValidMove, InvalidMove> ValidateMove(IPlayerState state, IMove move)
    {
      return state.RotateSpareTile(move.Rotation).PerformSlide(move.Slide).CanActivePlayerReach(move.Destination)
        ? new ValidMove()
        : new InvalidMove("Active player cannot reach this tile.");
    }
  }
}