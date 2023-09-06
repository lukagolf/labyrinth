using Common;
using LanguageExt;

namespace Players
{
  public class LastSlideRule : IRule
  {
    public Either<ValidMove, InvalidMove> ValidateMove(IPlayerState state, IMove move)
    {
      Option<ISlideAction> maybeSlide = state.LastSlideAction;
      ISlideAction newSlide = move.Slide;

      return maybeSlide
        .Some(lastSlide => ValidateSlide(lastSlide, newSlide))
        .None(() => new ValidMove());
    }

    private static Either<ValidMove, InvalidMove> ValidateSlide(ISlideAction lastSlide, ISlideAction newSlide)
    {
      return (AreOppositeSlides(lastSlide.Type, newSlide.Type) && lastSlide.Index == newSlide.Index)
        ? new InvalidMove("Move is invalid because it undoes the last slide.")
        : new ValidMove();
    }

    private static bool AreOppositeSlides(SlideType slideType1, SlideType slideType2)
    {
      return (slideType1 == SlideType.SlideRowLeft && slideType2 == SlideType.SlideRowRight) ||
             (slideType1 == SlideType.SlideRowRight && slideType2 == SlideType.SlideRowLeft) ||
             (slideType1 == SlideType.SlideColumnUp && slideType2 == SlideType.SlideColumnDown) ||
             (slideType1 == SlideType.SlideColumnDown && slideType2 == SlideType.SlideColumnUp);
    }
  }
}