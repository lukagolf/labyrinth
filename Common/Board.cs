using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Common
{
  public class Board : IBoard
  {
    #region Fields

    /// <summary>
    /// Internal representation of the board, as a 2D array.
    /// </summary>
    private readonly ITile[,] _board;

    /// <summary>
    /// Index of the vertical dimension of 2D _board array
    /// </summary>
    private const int VerticalDimension = 0;

    /// <summary>
    /// Index of the horizontal dimension of 2D _board array
    /// </summary>
    private const int HorizontalDimension = 1;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructs a new BoardTests from the given matrix of tiles.
    /// </summary>
    /// <param name="matrix">Matrix of tiles</param>
    /// <exception cref="ArgumentException">
    /// If the height of the given matrix is not even,
    /// or if the width of the given matrix is not even
    /// </exception>
    public Board(ITile[,] matrix)
    {
      ValidateMatrix(matrix);
      int numOfRows = matrix.GetLength(VerticalDimension);
      int numOfColumns = matrix.GetLength(HorizontalDimension);
      _board = new ITile[numOfRows, numOfColumns];
      _board.Fill((rowIdx, colIdx) => matrix[rowIdx, colIdx]);
    }

    private static void ValidateMatrix(ITile[,] board)
    {
      int numOfRows = board.GetLength(VerticalDimension);
      if (numOfRows < 1)
      {
        throw new ArgumentException("The number of rows must be greater than or equal to 1.");
      }

      int numOfColumns = board.GetLength(HorizontalDimension);
      if (numOfColumns < 1)
      {
        throw new ArgumentException("The number of columns must be greater than or equal to 1.");
      }
    }

    #endregion

    #region Public Interface

    public int GetWidth()
    {
      return _board.GetLength(HorizontalDimension);
    }

    public int GetHeight()
    {
      return _board.GetLength(VerticalDimension);
    }

    public ITile GetTileAt(BoardPosition position)
    {
      if (!IsValidPosition(position))
      {
        throw CreateInvalidPositionException(nameof(position));
      }

      return GetCell(position);
    }

    public bool IsReachable(BoardPosition source, BoardPosition destination)
    {
      if (!IsValidPosition(source))
      {
        throw CreateInvalidPositionException(nameof(source));
      }

      if (!IsValidPosition(destination))
      {
        throw CreateInvalidPositionException(nameof(destination));
      }

      Node[,] graph = CreateGraphRepresentation();
      Node sourceNode = graph[source.RowIndex, source.ColumnIndex];
      Node destinationNode = graph[destination.RowIndex, destination.ColumnIndex];
      return sourceNode.CanReachToNode(destinationNode);
    }

    public IEnumerable<BoardPosition> GetAllReachablePositions(BoardPosition source)
    {
      return _board.Enumerate()
        .Where(triple => IsReachable(source, new BoardPosition(triple.rowIdx, triple.colIdx)))
        .Select(triple => new BoardPosition(triple.rowIdx, triple.colIdx))
        .ToHashSet();
    }

    public ITile SlideRowToLeft(ITile tileToInsert, int rowIndex)
    {
      ValidateSlideRow(rowIndex);
      return SlideRowToLeftInternal(tileToInsert, rowIndex);
    }

    public ITile SlideRowToRight(ITile tileToInsert, int rowIndex)
    {
      ValidateSlideRow(rowIndex);
      return SlideRowToRightInternal(tileToInsert, rowIndex);
    }

    public ITile SlideColumnToUp(ITile tileToInsert, int columnIndex)
    {
      ValidateSlideColumn(columnIndex);
      return SlideColumnToUpInternal(tileToInsert, columnIndex);
    }

    public ITile SlideColumnToDown(ITile tileToInsert, int columnIndex)
    {
      ValidateSlideColumn(columnIndex);
      return SlideColumnToDownInternal(tileToInsert, columnIndex);
    }

    public IEnumerable<BoardPosition> Positions =>
      _board.Enumerate().Select(triple => new BoardPosition(triple.rowIdx, triple.colIdx));

    public IEnumerable<int> SlidableRows => Enumerable.Range(0, GetHeight()).Where(IsEven);

    public IEnumerable<int> SlidableColumns => Enumerable.Range(0, GetWidth()).Where(IsEven);

    public bool HasTreasure(ITreasure treasure) => Positions.Any(p => GetTileAt(p).Treasure.Equals(treasure));

    #endregion

    #region Common Helpers

    private ITile GetCell(BoardPosition position)
    {
      return _board[position.RowIndex, position.ColumnIndex];
    }

    private bool IsValidPosition(BoardPosition position)
    {
      int maxRowIdx = _board.GetLength(VerticalDimension) - 1;
      int maxColIdx = _board.GetLength(HorizontalDimension) - 1;
      return position.RowIndex >= 0 && position.RowIndex <= maxRowIdx && position.ColumnIndex >= 0 &&
             position.ColumnIndex <= maxColIdx;
    }

    private static ArgumentException CreateInvalidPositionException(string argumentName)
    {
      return new($"Given board position '{argumentName}' is invalid.");
    }

    private bool IsValidRowIndex(int index)
    {
      return index >= 0 && index <= GetHeight() - 1;
    }

    private bool IsValidColumnIndex(int index)
    {
      return index >= 0 && index <= GetWidth() - 1;
    }

    private static bool IsEven(int n) => n % 2 == 0;

    #endregion

    #region Slide Helpers

    private void ValidateSlideRow(int rowIndex)
    {
      if (!IsValidRowIndex(rowIndex))
      {
        throw new ArgumentException("Row index is invalid.");
      }

      if (!IsRowOrColumnSlidable(rowIndex))
      {
        throw new ArgumentException("Only even row indices are slidable.");
      }
    }

    private void ValidateSlideColumn(int columnIndex)
    {
      if (!IsValidColumnIndex(columnIndex))
      {
        throw new ArgumentException("Column index is invalid.");
      }

      if (!IsRowOrColumnSlidable(columnIndex))
      {
        throw new ArgumentException("Only even column indices are slidable.");
      }
    }

    private bool IsRowOrColumnSlidable(int index)
    {
      return new BigInteger(index).IsEven;
    }

    private ITile SlideRowToLeftInternal(ITile tileToInsert, int rowIdx)
    {
      int leftMostColIdx = 0;
      int colIdxDelta = 1;
      return SlideRowHelp(tileToInsert, rowIdx, leftMostColIdx, colIdxDelta);
    }

    private ITile SlideRowToRightInternal(ITile tileToInsert, int rowIdx)
    {
      int rightMostColIdx = GetWidth() - 1;
      int colIdxDelta = -1;
      return SlideRowHelp(tileToInsert, rowIdx, rightMostColIdx, colIdxDelta);
    }

    private ITile SlideColumnToUpInternal(ITile tileToInsert, int colIdx)
    {
      int upperMostRowIdx = 0;
      int rowIdxDelta = 1;
      return SlideColumnHelp(tileToInsert, colIdx, upperMostRowIdx, rowIdxDelta);
    }

    private ITile SlideColumnToDownInternal(ITile tileToInsert, int colIdx)
    {
      int downMostRowIdx = GetHeight() - 1;
      int rowIdxDelta = -1;
      return SlideColumnHelp(tileToInsert, colIdx, downMostRowIdx, rowIdxDelta);
    }

    private ITile SlideRowHelp(ITile tileToInsert, int rowIdxToSlide, int edgeColIdx, int colIdxDelta)
    {
      ITile popoutTile = _board[rowIdxToSlide, edgeColIdx];
      int colIdx = edgeColIdx;
      int slideCount = 0;
      int numOfSlides = GetWidth() - 1;
      while (slideCount < numOfSlides)
      {
        _board[rowIdxToSlide, colIdx] = _board[rowIdxToSlide, colIdx + colIdxDelta];
        colIdx += colIdxDelta;
        slideCount += 1;
      }

      _board[rowIdxToSlide, colIdx] = tileToInsert;
      return popoutTile;
    }

    /// <summary>
    /// Slides the specified column up or down depending on the edge row and row delta
    /// 
    /// _board[edgeRowIdx, colIdxToSlide].ValueUnsafe() is safe to call because it is already checked that board
    /// does not have an empty spot <see cref="ValidateSlideColumn"/>>
    /// </summary>
    private ITile SlideColumnHelp(ITile tileToInsert, int colIdxToSlide, int edgeRowIdx, int rowIdxDelta)
    {
      ITile popoutTile = _board[edgeRowIdx, colIdxToSlide];
      int rowIdx = edgeRowIdx;
      int slideCount = 0;
      int numOfSlides = GetWidth() - 1;
      while (slideCount < numOfSlides)
      {
        _board[rowIdx, colIdxToSlide] = _board[rowIdx + rowIdxDelta, colIdxToSlide];
        rowIdx += rowIdxDelta;
        slideCount += 1;
      }

      _board[rowIdx, colIdxToSlide] = tileToInsert;
      return popoutTile;
    }

    #endregion

    #region IsReachable Helpers

    private Node[,] CreateGraphRepresentation()
    {
      Node[,] nodeMatrix = CreateNodeMatrix();
      EstablishConnections(nodeMatrix);
      return nodeMatrix;
    }

    private Node[,] CreateNodeMatrix()
    {
      var nodeMatrix = new Node[GetHeight(), GetWidth()];
      nodeMatrix.Fill((_, _) => new Node());
      return nodeMatrix;
    }

    private void EstablishConnections(Node[,] nodeMatrix)
    {
      nodeMatrix.ForEach((rowIdx, colIdx, _) =>
      {
        EstablishUpConnection(nodeMatrix, rowIdx, colIdx);
        EstablishLeftConnection(nodeMatrix, rowIdx, colIdx);
        EstablishDownConnection(nodeMatrix, rowIdx, colIdx);
        EstablishRightConnection(nodeMatrix, rowIdx, colIdx);
      });
    }

    private void EstablishUpConnection(Node[,] nodeMatrix, int rowIdx, int colIdx)
    {
      if (rowIdx <= 0)
      {
        return;
      }

      ITile currentTile = _board[rowIdx, colIdx];
      Node currentNode = nodeMatrix[rowIdx, colIdx];
      ITile upTile = _board[rowIdx - 1, colIdx];
      Node upNode = nodeMatrix[rowIdx - 1, colIdx];
      if (currentTile.HasUpConnection() && upTile.HasDownConnection())
      {
        currentNode.AddNeighbor(upNode);
      }
    }

    private void EstablishLeftConnection(Node[,] nodeMatrix, int rowIdx, int colIdx)
    {
      if (colIdx <= 0)
      {
        return;
      }

      ITile currentTile = _board[rowIdx, colIdx];
      Node currentNode = nodeMatrix[rowIdx, colIdx];
      ITile leftTile = _board[rowIdx, colIdx - 1];
      Node leftNode = nodeMatrix[rowIdx, colIdx - 1];
      if (currentTile.HasLeftConnection() && leftTile.HasRightConnection())
      {
        currentNode.AddNeighbor(leftNode);
      }
    }

    private void EstablishDownConnection(Node[,] nodeMatrix, int rowIdx, int colIdx)
    {
      if (rowIdx >= GetHeight() - 1)
      {
        return;
      }

      ITile currentTile = _board[rowIdx, colIdx];
      Node currentNode = nodeMatrix[rowIdx, colIdx];
      ITile downTile = _board[rowIdx + 1, colIdx];
      Node downNode = nodeMatrix[rowIdx + 1, colIdx];
      if (currentTile.HasDownConnection() && downTile.HasUpConnection())
      {
        currentNode.AddNeighbor(downNode);
      }
    }

    private void EstablishRightConnection(Node[,] nodeMatrix, int rowIdx, int colIdx)
    {
      if (colIdx >= GetHeight() - 1)
      {
        return;
      }

      ITile currentTile = _board[rowIdx, colIdx];
      Node currentNode = nodeMatrix[rowIdx, colIdx];
      ITile rightTile = _board[rowIdx, colIdx + 1];
      Node rightNode = nodeMatrix[rowIdx, colIdx + 1];
      if (currentTile.HasRightConnection() && rightTile.HasLeftConnection())
      {
        currentNode.AddNeighbor(rightNode);
      }
    }

    private class Node
    {
      private readonly IList<Node> _neighbors;

      public Node()
      {
        _neighbors = new List<Node>();
      }

      public void AddNeighbor(Node node)
      {
        _neighbors.Add(node);
      }

      public bool CanReachToNode(Node node)
      {
        return CanReachToNodeAcc(node, new HashSet<Node>());
      }

      private bool CanReachToNodeAcc(Node node, ISet<Node> visited)
      {
        visited.Add(this);
        var unvisitedNeighbors = _neighbors.Where(n => !visited.Contains(n)).ToList();
        return this == node || unvisitedNeighbors.Any(n => n == node) ||
               unvisitedNeighbors.Any(n => n.CanReachToNodeAcc(node, visited));
      }
    }

    #endregion
  }
}