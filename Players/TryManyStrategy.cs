using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that combines multiple strategies. The first strategy that produces a move will be used.
  /// </summary>
  public sealed class TryManyStrategy : IPlayerStrategy
  {
    /// <summary>
    /// The list of all strategies sorted in the ascending order of priority. The first strategy is the most desirable
    /// and the bottommost strategy is least desirable.
    /// </summary>
    private readonly IList<IPlayerStrategy> _strategies;

    /// <summary>
    /// Constructs a new combined strategy from the list of given strategies. The first strategy in the list gets
    /// the top priority.
    /// </summary>
    /// <param name="strategies">The list of given strategies</param>
    public TryManyStrategy(IEnumerable<IPlayerStrategy> strategies)
    {
      _strategies = strategies.ToList();
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      Option<IPlayerStrategy> strategy = _strategies.Find(strategy => strategy.ChooseMove(state, rule, goal).IsSome);
      return strategy
        .Some(s => s.ChooseMove(state, rule, goal))
        .None(() => Option<Either<IMove, Pass>>.None);
    }
  }
}