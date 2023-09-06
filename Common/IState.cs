using System;
using System.Collections.Generic;
using LanguageExt;

namespace Common
{
  /// <summary>
  /// A generic interface that represents the common functionality of referee's and players' states.
  /// </summary>
  public interface IState
  {
    /// <summary>
    /// Gets the game board
    /// </summary>
    public IImmutableBoard Board { get; }

    /// <summary>
    /// Gets the current spare tile
    /// </summary>
    public ITile SpareTile { get; }

    /// <summary>
    /// Gets the last slide action performed by the last active player, if there is one
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If none of the players made a move yet
    /// </exception>
    public Option<ISlideAction> LastSlideAction { get; }

    /// <summary>
    /// Gets the public information of the player who has the current turn
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If there are no players left in the game
    /// </exception>
    public IImmutablePublicPlayerInfo ActivePlayer { get; }

    /// <summary>
    /// Can the currently active player reach the given destination from its current position?
    /// </summary>
    /// <param name="destination">The destination position to check</param>
    /// <returns>
    /// Whether the currently active player can reach the given destination from its current position
    /// </returns>
    public bool CanActivePlayerReach(BoardPosition destination);

    /// <summary>
    /// Gets all reachable positions by the currently active player in the row-column order.
    /// </summary>
    public IEnumerable<BoardPosition> AllReachablePositionsByActivePlayer { get; }

    /// <summary>
    /// Gets the public information of all the players who are currently playing the game (in their turn order)
    /// </summary>
    public IEnumerable<IImmutablePublicPlayerInfo> AllPlayers { get; }

    /// <summary>
    /// Gets the public information of the player with the given color
    /// </summary>
    /// <param name="color">The color of the player to be returned</param>
    /// <returns>The public information of the player with the given color</returns>
    public IImmutablePublicPlayerInfo GetPlayer(Color color);
  }
}