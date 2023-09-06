using System;
using System.Reflection.Metadata;
using System.Threading;
using Common;
using LanguageExt;

namespace Players
{
  public class BadPlayer : IPlayer
  {
    private readonly IPlayerStrategy _strategy;
    private readonly IBoardBuilder _boardBuilder;
    private readonly BadConfig _badConfig;
    private readonly IRule _rule;
    private Option<BoardPosition> _maybeGoal;
    private int _setupCount;
    private int _takeTurnCount;
    private int _wonCount;


    public BadPlayer(string name, IPlayerStrategy strategy, IBoardBuilder boardBuilder, BadConfig badConfig)
    {
      Name = name;
      _strategy = strategy;
      _boardBuilder = boardBuilder;
      _badConfig = badConfig;
      _rule = new RuleBook();
      _maybeGoal = Option<BoardPosition>.None;
      _setupCount = 0;
      _takeTurnCount = 0;
      _wonCount = 0;
    }

    public string Name { get; }

    public IBoard ProposeBoard(int rows, int columns)
    {
      return _boardBuilder.BuildBoard(rows, columns);
    }

    public Acknowledge Setup(Option<IPlayerState> initialState, BoardPosition goal)
    {
      _setupCount += 1;
      if (_badConfig.BadMethod == BadMethod.Setup && _setupCount >= _badConfig.Count)
      {
        InvokeBadAction(_badConfig.BadAction);
      }

      _maybeGoal = goal;
      return Acknowledge.Value;
    }

    public Either<IMove, Pass> TakeTurn(IPlayerState state)
    {
      _takeTurnCount += 1;
      if (_badConfig.BadMethod == BadMethod.TakeTurn && _takeTurnCount >= _badConfig.Count)
      {
        InvokeBadAction(_badConfig.BadAction);
      }

      Either<IMove, Pass> moveOrPass = _maybeGoal
        .Some(goal => _strategy.ChooseMove(state, _rule, goal).ToMaybeMove())
        .None(() => Either<IMove, Pass>.Right(Pass.Value));

      return moveOrPass;
    }

    public Acknowledge Won(bool won)
    {
      _wonCount += 1;
      if (_badConfig.BadMethod == BadMethod.Won && _wonCount >= _badConfig.Count)
      {
        InvokeBadAction(_badConfig.BadAction);
      }

      return Acknowledge.Value;
    }

    private static void InvokeBadAction(BadAction badAction)
    {
      switch (badAction)
      {
        case BadAction.ThrowException:
          throw new Exception("Mock Exception");
        case BadAction.TakeTooLong:
          GoInfinite();
          break;
      }
    }

    private static void GoInfinite()
    {
      while (true)
      {
        Thread.Sleep(100);
      }
      // ReSharper disable once FunctionNeverReturns
    }

    public sealed class BadConfig
    {
      public BadConfig(BadMethod badMethod, BadAction badAction, int count)
      {
        BadMethod = badMethod;
        BadAction = badAction;
        Count = count;
      }

      public BadMethod BadMethod { get; }
      public BadAction BadAction { get; }
      public int Count { get; }
    }

    public enum BadMethod
    {
      Setup,
      TakeTurn,
      Won
    }

    public enum BadAction
    {
      ThrowException,
      TakeTooLong
    }
  }
}