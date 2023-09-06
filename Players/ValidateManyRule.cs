using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;

namespace Players
{
  public class ValidateManyRule : IRule
  {
    private readonly IList<IRule> _rules;

    public ValidateManyRule(IEnumerable<IRule> rules)
    {
      _rules = rules.ToList();
    }

    public Either<ValidMove, InvalidMove> ValidateMove(IPlayerState state, IMove move)
    {
      Option<IRule> result = _rules.Find(rule => rule.ValidateMove(state, move).IsRight);
      return result
        .Some(rule => rule.ValidateMove(state, move))
        .None(() => new ValidMove());
    }
  }
}