using System;

namespace Common
{
  /// <summary>
  /// Represents mutable operations that a maze board must support
  /// </summary>
  public interface IBoard : IImmutableBoard
  {
    /// <summary>
    /// Slides the row at the given row index one tile to the left by inserting the given tile to the rightmost column,
    /// and returns the tile that gets out of the board
    /// </summary>
    /// <param name="tileToInsert">The tile to insert</param>
    /// <param name="rowIndex">The index of the row to slide, starting from 0</param>
    /// <returns>The tile that gets out of the board</returns>
    /// <exception cref="ArgumentException">
    /// If the given row index is invalid (out of bounds), or the row is not slidable
    /// </exception>
    public ITile SlideRowToLeft(ITile tileToInsert, int rowIndex);

    /// <summary>
    /// Slides the row at the given row index one tile to the right by inserting the given tile to the leftmost column,
    /// and returns the tile that gets out of the board
    /// </summary>
    /// <param name="tileToInsert">The tile to insert</param>
    /// <param name="rowIndex">The index of the row to slide, starting from 0</param>
    /// <returns>The tile that gets out of the board</returns>
    /// <exception cref="ArgumentException">
    /// If the given row index is invalid (out of bounds), or the row is not slidable
    /// </exception>
    public ITile SlideRowToRight(ITile tileToInsert, int rowIndex);

    /// <summary>
    /// Slides the column at the given column index one tile upwards by inserting the given tile to the bottommost row,
    /// and returns the tile that gets out of the board
    /// </summary>
    /// <param name="tileToInsert">The tile to insert</param>
    /// <param name="columnIndex">The index of the column to slide, starting from 0</param>
    /// <returns>The tile that gets out of the board</returns>
    /// <exception cref="ArgumentException">
    /// If the given column index is invalid (out of bounds), or the column is not slidable
    /// </exception>
    public ITile SlideColumnToUp(ITile tileToInsert, int columnIndex);
    
    /// <summary>
    /// Slides the column at the given column index one tile downwards by inserting the given tile to the upmost row,
    /// and returns the tile that gets out of the board
    /// </summary>
    /// <param name="tileToInsert">The tile to insert</param>
    /// <param name="columnIndex">The index of the column to slide, starting from 0</param>
    /// <returns>The tile that gets out of the board</returns>
    /// <exception cref="ArgumentException">
    /// If the given column index is invalid (out of bounds), or the column is not slidable
    /// </exception>
    public ITile SlideColumnToDown(ITile tileToInsert, int columnIndex);
  }
}