using Common;
using LanguageExt;

namespace Players
{
  public interface IRule
  {
    public Either<ValidMove, InvalidMove> ValidateMove(IPlayerState state, IMove move);
  }

  public sealed class ValidMove
  {
  }

  public sealed class InvalidMove
  {
    public InvalidMove(string explanation)
    {
      Explanation = explanation;
    }

    public string Explanation { get; }
  }
}