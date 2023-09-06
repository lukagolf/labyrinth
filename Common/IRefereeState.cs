using System;
using System.Collections.Generic;

namespace Common
{
  /// <summary>
  /// Represents the referee's game state. It extends IState with additional functionalities that allow the referee
  /// to run the game from start to finish and access private player information.
  /// </summary>
  public interface IRefereeState : IState
  {
    /// <summary>
    /// Rotates the spare tile in clock-wise direction by the given number of degrees
    /// </summary>
    /// <param name="rotation">The number of degrees to rotate by</param>
    public void RotateSpareTile(Rotation rotation);

    /// <summary>
    /// Performs the given slide action on this state
    /// </summary>
    /// <param name="slideAction">The slide action to perform</param>
    public void PerformSlide(ISlideAction slideAction);

    /// <summary>
    /// Removes the currently active player
    /// </summary>
    /// <returns>The player that has been removed from the game</returns>
    public IPlayerInfo RemoveActivePlayer();

    /// <summary>
    /// Removes the given player from the state
    /// </summary>
    /// <param name="player">The player to remove</param>
    public void RemovePlayer(IPlayerInfo player);

    /// <summary>
    /// Changes the currently active player to the next player in the turn order
    /// </summary>
    public void NextTurn();

    /// <summary>
    /// Has active player reached treasure?
    /// </summary>
    public bool HasActivePlayerReachedTreasure();

    /// <summary>
    /// Has active player reached home?
    /// </summary>
    public bool HasActivePlayerReachedHome();

    /// <summary>
    /// Moves the currently active player to the given board position
    /// </summary>
    /// <param name="destination">The new position of the currently active player</param>
    /// <exception cref="ArgumentException">If the new player position is not on the board</exception>
    public void MoveActivePlayer(BoardPosition destination);

    /// <summary>
    /// Creates a deep defensive copy of the game state.
    /// </summary>
    /// <returns>A defensive copy of the game state</returns>
    public IRefereeState Copy();

    /// <summary>
    /// Creates the projection of this game state onto a new player state
    /// </summary>
    /// <returns>A projection of this game state onto a new player state</returns>
    public IPlayerState ToPlayerState();

    public new IPlayerInfo ActivePlayer { get; }

    public new IEnumerable<IPlayerInfo> AllPlayers { get; }

    public new IPlayerInfo GetPlayer(Color color);

    public int NumberOfTreasures { get; }
    public ITreasure NextTreasure { get; }
    public ITreasure PopNextTreasure();
  }
}