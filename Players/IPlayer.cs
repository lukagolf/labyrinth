using Common;
using LanguageExt;

namespace Players
{
  /// <summary>
  /// Represents a player that referee will communicate with
  /// </summary>
  public interface IPlayer
  {
    /// <summary>
    /// Returns the name of this player
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns the board this player wants to propose
    /// </summary>
    /// <param name="rows">The number of rows of the board that the player should propose</param>
    /// <param name="columns">The number of columns of the board that the player should propose</param>
    /// <returns>The player's proposed board</returns>
    public IBoard ProposeBoard(int rows, int columns);

    /// <summary>
    /// This method serves two purposes:
    /// 1 - If initial state is not none, it informs the player that the game has started and the player's goal
    /// 2- If the initial state is none, it reminds the player where home is
    /// </summary>
    /// <param name="initialState">The initial state of the game</param>
    /// <param name="goal">The position the player should try to reach, it is either the goal or the home</param>
    /// <returns>An acknowledgement that the player has received the setup information</returns>
    public Acknowledge Setup(Option<IPlayerState> initialState, BoardPosition goal);

    /// <summary>
    /// Retrieves the player's desired action given the current state of the game
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <returns>
    /// Either:
    /// - IMove: If the player chooses to make a move
    /// - Pass: If the player chooses to pass
    /// </returns>
    public Either<IMove, Pass> TakeTurn(IPlayerState state);

    /// <summary>
    /// Informs the player that the game is over and if it won or not
    /// </summary>
    /// <param name="won">Did the player win the game?</param>
    /// <returns>An acknowledgement that the player has received the game over information</returns>
    public Acknowledge Won(bool won);
  }
}