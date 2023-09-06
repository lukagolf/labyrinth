using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a player that makes up its mind using a strategy
  /// </summary>
  public sealed class Player : IPlayer
  {
    /// <summary>
    /// The name of the player
    /// </summary>
    private readonly string _name;
    
    /// <summary>
    /// The strategy this player will use when taking turn
    /// </summary>
    private readonly IPlayerStrategy _strategy;
    
    /// <summary>
    /// The game rules this player will use to check the validity of its moves
    /// </summary>
    private readonly IRule _rule;
    
    /// <summary>
    /// Represents either the position of the goal, or the home tile
    /// </summary>
    private Option<BoardPosition> _maybeGoal;
    
    /// <summary>
    /// The player will use this builder to construct a board during propose board stage
    /// </summary>
    private readonly IBoardBuilder _boardBuilder;

    public Player(string name, IPlayerStrategy strategy, IBoardBuilder boardBuilder) : this(name, strategy,
      boardBuilder, new RuleBook())
    {
    }

    public Player(string name, IPlayerStrategy strategy, IBoardBuilder boardBuilder, IRule rule)
    {
      _name = name;
      _strategy = strategy;
      _rule = rule;
      _boardBuilder = boardBuilder;
      _maybeGoal = Option<BoardPosition>.None;
    }

    public string Name => _name;

    public IBoard ProposeBoard(int rows, int columns)
    {
      return _boardBuilder.BuildBoard(rows, columns);
    }

    public Acknowledge Setup(Option<IPlayerState> initialState, BoardPosition goal)
    {
      _maybeGoal = goal;
      return Acknowledge.Value;
    }

    public Either<IMove, Pass> TakeTurn(IPlayerState state)
    {
      Either<IMove, Pass> moveOrPass = _maybeGoal
        .Some(goal => _strategy.ChooseMove(state, _rule, goal).ToMaybeMove())
        .None(() => Either<IMove, Pass>.Right(Pass.Value));

      return moveOrPass;
    }

    public Acknowledge Won(bool won)
    {
      return Acknowledge.Value;
    }
  }
}