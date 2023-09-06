using System.Collections.Generic;
using Common;
using LanguageExt;

namespace Players
{
  public class RuleBook : IRule
  {
    private readonly IRule _internalRule;

    public RuleBook()
    {
      _internalRule = new ValidateManyRule(
        new List<IRule>
        {
          new ReachableRule(),
          new LastSlideRule(),
          new CannotStayOnTheSamePositionRule()
        }
      );
    }

    public Either<ValidMove, InvalidMove> ValidateMove(IPlayerState state, IMove move)
    {
      return _internalRule.ValidateMove(state, move);
    }
  }
}