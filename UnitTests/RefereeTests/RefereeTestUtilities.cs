using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using LanguageExt;
using Players;

namespace UnitTests.RefereeTests
{
  public static class RefereeTestUtilities
  {
    internal static IRefereeState CreateGameState()
    {
      var purplePlayer = new PlayerInfo(Color.Purple, new BoardPosition(1, 1), new BoardPosition(1, 1),
        new Treasure(Gem.AlexandritePearShape, Gem.Goldstone));

      var orangePlayer = new PlayerInfo(Color.Orange, new BoardPosition(1, 3), new BoardPosition(1, 3),
        new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate));

      var pinkPlayer = new PlayerInfo(Color.Pink, new BoardPosition(1, 5), new BoardPosition(1, 5),
        new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique));

      var players = new List<IPlayerInfo> {purplePlayer, orangePlayer, pinkPlayer};

      ITile spareTile = new Tile(true, false, true, false,
        new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape));

      return new RefereeState(players, CreateBoard(), spareTile);
    }

    public static IRefereeState CreateGameStateWinByReachingHome()
    {
      var purplePlayer = new PlayerInfo(Color.Purple, new BoardPosition(1, 1), new BoardPosition(1, 1),
        new Treasure(Gem.AlexandritePearShape, Gem.Goldstone));

      var orangePlayer = new PlayerInfo(Color.Orange, new BoardPosition(1, 3), new BoardPosition(1, 3),
        new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate));

      var pinkPlayer = new PlayerInfo(Color.Pink, new BoardPosition(1, 5), new BoardPosition(0, 0),
        new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique));

      var players = new List<IPlayerInfo> {purplePlayer, orangePlayer, pinkPlayer};

      ITile spareTile = new Tile(true, false, true, false,
        new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape));

      return new RefereeState(players, CreateBoard(), spareTile);
    }

    public static IRefereeState CreateGameStateWithExtraTreasures(IList<ITreasure> treasures)
    {
      var purplePlayer = new PlayerInfo(Color.Purple, new BoardPosition(1, 1), new BoardPosition(1, 1),
        new Treasure(Gem.AlexandritePearShape, Gem.Goldstone));

      var orangePlayer = new PlayerInfo(Color.Orange, new BoardPosition(1, 3), new BoardPosition(1, 3),
        new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate));

      var pinkPlayer = new PlayerInfo(Color.Pink, new BoardPosition(1, 5), new BoardPosition(0, 0),
        new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique));

      var players = new List<IPlayerInfo> {purplePlayer, orangePlayer, pinkPlayer};

      ITile spareTile = new Tile(true, false, true, false,
        new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape));

      return new RefereeState(players, CreateBoard(), spareTile, Option<ISlideAction>.None, treasures);
    }

    private static IBoard CreateBoard()
    {
      return new Board(new ITile[,]
      {
        {
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.AlmandineGarnet)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Amethyst)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Ametrine)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Ammolite)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Apatite)),
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Aplite)),
        },

        {
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.ApricotSquareRadiant)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Aquamarine)),
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.AustralianMarquise)),
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Aventurine)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Azurite)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Beryl)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.BlackObsidian)),
        },

        {
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BlackOnyx)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.BlackSpinelCushion)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BlueCeylonSapphire)),
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BlueCushion)),
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.BluePearShape)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.BlueSpinelHeart)),
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BullsEye)),
        },

        {
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Carnelian)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.ChromeDiopside)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.ChrysoberylCushion)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Chrysolite)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.CitrineCheckerboard)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Citrine)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Clinohumite)),
        },

        {
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.ColorChangeOval)),
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Cordierite)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Diamond)),
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Dumortierite)),
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Emerald)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.FancySpinelMarquise)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Garnet)),
        },

        {
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.GoldenDiamondCut)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Goldstone)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Grandidierite)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate)),
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.GreenAventurine)),
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.GreenBeryl)),
        },

        {
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.GreenPrincessCut)),
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.GrossularGarnet)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Hackmanite)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Heliotrope)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Hematite)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.IoliteEmeraldCut)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Jasper)),
        },
      });
    }
  }

  public sealed class MockPlayer : IPlayer
  {
    private readonly IPlayerStrategy _strategy;
    private readonly IRule _rule;
    private Option<BoardPosition> _maybeGoal;

    // mock player fields
    public bool PlayerWon;
    public bool CalledWon;
    public bool CalledSetup;
    public bool CalledTakeTurn;
    public int NumberOfTurns;

    public bool ThrowExceptionInSetup;
    public bool TakeTooLongInSetup;

    public bool ThrowExceptionInWon;
    public bool TakeTooLongInWon;

    public MockPlayer(string name, IPlayerStrategy strategy)
    {
      Name = name;
      _strategy = strategy;
      _rule = new RuleBook();
      _maybeGoal = Option<BoardPosition>.None;

      PlayerWon = false;
      CalledWon = false;
      CalledSetup = false;
      CalledTakeTurn = false;
      NumberOfTurns = 0;
    }

    public string Name { get; }

    public IBoard ProposeBoard(int rows, int columns)
    {
      throw new NotImplementedException();
    }

    public Acknowledge Setup(Option<IPlayerState> initialState, BoardPosition goal)
    {
      CalledSetup = true;
      _maybeGoal = goal;

      if (ThrowExceptionInSetup)
      {
        throw new Exception("Mock Exception");
      }

      if (TakeTooLongInSetup)
      {
        Thread.Sleep(1500);
      }

      return Acknowledge.Value;
    }

    public Either<IMove, Pass> TakeTurn(IPlayerState state)
    {
      CalledTakeTurn = true;
      NumberOfTurns += 1;
      var maybeMove = _maybeGoal
        .Some(goal => _strategy.ChooseMove(state, _rule, goal).ToMaybeMove())
        .None(() => Either<IMove, Pass>.Right(Pass.Value));

      return maybeMove;
    }

    public Acknowledge Won(bool won)
    {
      CalledWon = true;
      PlayerWon = won;

      if (ThrowExceptionInWon)
      {
        throw new Exception("Mock exception.");
      }

      if (TakeTooLongInWon)
      {
        Thread.Sleep(1500);
      }

      return Acknowledge.Value;
    }
  }

  /// <summary>
  /// In the first ChooseMove call, make a simple move, in the second ChooseMove call, say Pass.
  /// </summary>
  public sealed class MoveAndThenPassStrategy : IPlayerStrategy
  {
    private readonly IMove _move;
    private bool _sayPass;

    public MoveAndThenPassStrategy(IMove move)
    {
      _move = move;
      _sayPass = false;
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      if (_sayPass)
      {
        return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Right(Pass.Value));
      }

      _sayPass = true;
      return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Left(_move));
    }
  }

  public sealed class ToggleMoveStrategy : IPlayerStrategy
  {
    private readonly IList<IMove> _moves;


    public ToggleMoveStrategy(IMove move1, IMove move2)
    {
      _moves = new List<IMove> {move1, move2};
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      IMove firstMove = _moves.First();
      _moves.RemoveAt(0);
      _moves.Add(firstMove);
      return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Left(firstMove));
    }
  }

  public sealed class ThrowExceptionStrategy : IPlayerStrategy
  {
    private readonly int _throwAfter;
    private int _roundCount;

    public ThrowExceptionStrategy(int throwAfter)
    {
      _throwAfter = throwAfter;
      _roundCount = 0;
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      _roundCount += 1;
      if (_roundCount > _throwAfter)
      {
        throw new Exception("Mock strategy exception");
      }

      return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Right(Pass.Value));
    }
  }

  public sealed class TakeTooLongStrategy : IPlayerStrategy
  {
    private readonly int _waitAfter;
    private int _roundCount;
    private readonly IPlayerStrategy _internalStrategy;

    public TakeTooLongStrategy(IPlayerStrategy internalStrategy, int waitAfter)
    {
      _internalStrategy = internalStrategy;
      _waitAfter = waitAfter;
      _roundCount = 0;
    }


    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      _roundCount += 1;
      if (_roundCount > _waitAfter)
      {
        Thread.Sleep(1500);
      }

      return _internalStrategy.ChooseMove(state, rule, goal);
    }
  }

  /// <summary>
  /// Chooses to pass for the given number of rounds, then chooses to make the given move
  /// </summary>
  public sealed class PassThenMoveStrategy : IPlayerStrategy
  {
    private readonly IMove _move;
    private readonly int _moveAfter;
    private int _roundCount;

    public PassThenMoveStrategy(IMove move, int moveAfter)
    {
      _move = move;
      _moveAfter = moveAfter;
      _roundCount = 0;
    }

    public Option<Either<IMove, Pass>> ChooseMove(IPlayerState state, IRule rule, BoardPosition goal)
    {
      _roundCount += 1;
      if (_roundCount > _moveAfter)
      {
        return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Left(_move));
      }

      return Option<Either<IMove, Pass>>.Some(Either<IMove, Pass>.Right(Pass.Value));
    }
  }
}