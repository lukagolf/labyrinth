using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a strategy that always passes
  /// </summary>
  public class PassStrategy : IPlayerStrategy
  {
    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Right(Pass.Value));
    }
  }
}