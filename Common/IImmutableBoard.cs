using System;
using System.Collections.Generic;

namespace Common
{
  /// <summary>
  /// Represents immutable operations that a maze board must support
  /// </summary>
  public interface IImmutableBoard
  {
    /// <summary>
    /// Returns the width of the board
    /// </summary>
    /// <returns>The width of the board</returns>
    public int GetWidth();

    /// <summary>
    /// Returns the height of the board
    /// </summary>
    /// <returns>The height of the board</returns>
    public int GetHeight();

    /// <summary>
    /// Retrieves the tile at the given position if the position is valid and there exists a tile at that position
    /// </summary>
    /// <param name="position">The position on the board</param>
    /// <returns>The tile at the given position</returns>
    /// <exception cref="ArgumentException">
    /// If position is invalid (out of bounds) or there is no tile at that position
    /// </exception>
    public ITile GetTileAt(BoardPosition position);

    /// <summary>
    /// Is the tile at the given destination position reachable from the tile at the given source position?
    /// </summary>
    /// <param name="source">The position of the source tile on the board</param>
    /// <param name="destination">The position of the destination tile on the board</param>
    /// <returns>Whether tiles are reachable</returns>
    /// <exception cref="ArgumentException">
    /// If any of the given positions is invalid (out of bounds)
    /// </exception>
    public bool IsReachable(BoardPosition source, BoardPosition destination);

    /// <summary>
    /// Returns a set of all reachable tiles from the given source tile position
    /// </summary>
    /// <param name="source">The position of the source tile</param>
    /// <returns>A set of all reachable tiles from the given source tile position</returns>
    /// <exception cref="ArgumentException">
    /// If the given position is invalid (out of bounds)
    /// </exception>
    public IEnumerable<BoardPosition> GetAllReachablePositions(BoardPosition source);

    /// <summary>
    /// Iterates the board positions by following row-column order.
    /// </summary>
    public IEnumerable<BoardPosition> Positions { get; }
    
    /// <summary>
    /// Gets all slidable rows indices in this board in ascending order
    /// </summary>
    public IEnumerable<int> SlidableRows { get; }
    
    /// <summary>
    /// Gets all slidable column indices in this board in ascending order
    /// </summary>
    public IEnumerable<int> SlidableColumns { get; }

    /// <summary>
    /// Does the given treasure exist in this board?
    /// </summary>
    /// <param name="treasure">The treasure to check</param>
    /// <returns>true if there's a given treasure in this board, false otherwise</returns>
    public bool HasTreasure(ITreasure treasure);
  }
}