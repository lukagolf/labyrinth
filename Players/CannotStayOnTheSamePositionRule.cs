using Common;
using LanguageExt;

namespace Players
{
  public class CannotStayOnTheSamePositionRule : IRule
  {
    public Either<ValidMove, InvalidMove> ValidateMove(IPlayerState state, IMove move)
    {
      IPlayerState newState = state.RotateSpareTile(move.Rotation).PerformSlide(move.Slide);

      return newState.ActivePlayer.CurrentPosition.Equals(move.Destination)
        ? new InvalidMove(
          "Move is invalid because a player may not remain on the tile where it is located in this re-configured board.")
        : new ValidMove();
    }
  }
}