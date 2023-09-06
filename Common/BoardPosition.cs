using System;

namespace Common
{
  /// <summary>
  /// Represents a position on the board
  /// Interpretation: BoardPosition(0, 0) represents the top left corner of the board.
  /// rowIndex increases downwards and columnIndex increases to the right
  /// </summary>
  public readonly struct BoardPosition
  {
    public BoardPosition(int rowIndex, int columnIndex)
    {
      RowIndex = rowIndex;
      ColumnIndex = columnIndex;
    }

    /// <summary>
    /// Represents the row index starting from 0. Row index increases downwards.
    /// </summary>
    public int RowIndex { get; }

    /// <summary>
    /// Represents the column index starting from 0. Column index increases to the right.
    /// </summary>
    public int ColumnIndex { get; }

    public int EuclidDistance(BoardPosition other)
    {
      // This cast is safe because the result will always be an integer
      return (int) (Math.Pow((other.RowIndex - RowIndex), 2) + Math.Pow((other.ColumnIndex - ColumnIndex), 2));
    }
  }
}