using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents the strategy of a player. The strategy determines how a player will move given the game state.
  /// </summary>
  public interface IPlayerStrategy
  {
    /// <summary>
    /// Chooses the best move according to the implemented strategy
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <param name="rule">The rules that will be used to validate the moves this strategy comes up with</param>
    /// <param name="goal">The board position this strategy tries to reach</param>
    /// <returns>
    /// One Of:
    /// - Either(IMove, Pass): If this strategy wants to make a move or take a pass
    /// - None: If this strategy fails to make a decision
    /// </returns>
    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal);
  }

  public class Pass
  {
    private Pass()
    {
    }
    
    public static readonly Pass Value = new();
  }

  public static class StrategyResultExtensions
  {
    public static Either<IMove, Pass> ToMaybeMove(this Option<Either<IMove, Pass>> result)
    {
      return result
        .Some(maybeMove => maybeMove)
        .None(() => Either<IMove, Pass>.Right(Pass.Value));
    }
  }
}