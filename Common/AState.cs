using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Common
{
  /// <summary>
  /// An abstract class that implements the common functionality of referee's and players' states.
  /// </summary>
  /// <typeparam name="TPi">Represents the internal state information of players.</typeparam>
  public abstract class AState<TPi> : IState where TPi : IPublicPlayerInfo
  {
    private readonly IBoard _board;
    
    /// <summary>
    /// Internal representation of the order of players' turns during one cycle of turns
    /// </summary>
    private protected Queue<TPi> PlayerQueue;
    
    /// <param name="players">All players in the game</param>
    /// <param name="board">The board to be used in the game</param>
    /// <param name="spareTile">The current spare tile</param>
    /// <param name="lastSlideAction">The last slide action performed by the player</param>
    /// <param name="distinctHomes">Check for distinct homes?</param>
    private protected AState(IList<TPi> players, IImmutableBoard board, ITile spareTile,
      Option<ISlideAction> lastSlideAction, bool distinctHomes)
    {
      ValidatePlayers(players, board, distinctHomes);
      PlayerQueue = new Queue<TPi>(players);
      _board = CopyBoard(board);
      SpareTile = spareTile;
      LastSlideAction = lastSlideAction;
    }

    /// <summary>
    /// Internal representation of the board
    /// </summary>
    public IImmutableBoard Board => _board;

    public ITile SpareTile { get; private set; }
    public Option<ISlideAction> LastSlideAction { get; private set; }

    public IImmutablePublicPlayerInfo ActivePlayer => PlayerQueue.Peek();

    public bool CanActivePlayerReach(BoardPosition destination)
    {
      return Board.IsReachable(ActivePlayer.CurrentPosition, destination);
    }

    public IEnumerable<BoardPosition> AllReachablePositionsByActivePlayer =>
      Board.GetAllReachablePositions(ActivePlayer.CurrentPosition);

    public IEnumerable<IImmutablePublicPlayerInfo> AllPlayers => (IEnumerable<IImmutablePublicPlayerInfo>) PlayerQueue;

    public IImmutablePublicPlayerInfo GetPlayer(Color color) => AllPlayers
      .Find(player => player.Color.Equals(color))
      .Some(player => player)
      .None(() => throw new ArgumentException("Player with the given colorr does not exist."));

    private protected void RotateSpareTile(Rotation rotation)
    {
      SpareTile = SpareTile.Rotate(rotation);
    }

    private protected void PerformSlide(ISlideAction slideAction)
    {
      switch (slideAction.Type)
      {
        case SlideType.SlideRowLeft:
          SlideRowToLeft(slideAction.Index);
          break;
        case SlideType.SlideRowRight:
          SlideRowToRight(slideAction.Index);
          break;
        case SlideType.SlideColumnUp:
          SlideColumnToUp(slideAction.Index);
          break;
        case SlideType.SlideColumnDown:
          SlideColumnToDown(slideAction.Index);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(slideAction.Type));
      }
    }

    private void SlideRowToLeft(int rowIndex)
    {
      SpareTile = _board.SlideRowToLeft(SpareTile, rowIndex);
      UpdateAllPlayerPositions(position => UpdatePlayerPositionSlideRowLeft(position, rowIndex));
      LastSlideAction = new SlideAction(SlideType.SlideRowLeft, rowIndex);
    }

    private void SlideRowToRight(int rowIndex)
    {
      SpareTile = _board.SlideRowToRight(SpareTile, rowIndex);
      UpdateAllPlayerPositions(position => UpdatePlayerPositionSlideRowRight(position, rowIndex));
      LastSlideAction = new SlideAction(SlideType.SlideRowRight, rowIndex);
    }

    private void SlideColumnToUp(int columnIndex)
    {
      SpareTile = _board.SlideColumnToUp(SpareTile, columnIndex);
      UpdateAllPlayerPositions(position => UpdatePlayerPositionSlideColumnUp(position, columnIndex));
      LastSlideAction = new SlideAction(SlideType.SlideColumnUp, columnIndex);
    }

    private void SlideColumnToDown(int columnIndex)
    {
      SpareTile = _board.SlideColumnToDown(SpareTile, columnIndex);
      UpdateAllPlayerPositions(position => UpdatePlayerPositionSlideColumnDown(position, columnIndex));
      LastSlideAction = new SlideAction(SlideType.SlideColumnDown, columnIndex);
    }

    private BoardPosition UpdatePlayerPositionSlideRowLeft(BoardPosition playerPosition, int rowIdx)
    {
      if (playerPosition.RowIndex != rowIdx)
      {
        return playerPosition;
      }

      return playerPosition.ColumnIndex == 0
        ? new BoardPosition(rowIdx, Board.GetWidth() - 1)
        : new BoardPosition(rowIdx, playerPosition.ColumnIndex - 1);
    }

    private BoardPosition UpdatePlayerPositionSlideRowRight(BoardPosition playerPosition, int rowIdx)
    {
      if (playerPosition.RowIndex != rowIdx)
      {
        return playerPosition;
      }

      return playerPosition.ColumnIndex == Board.GetWidth() - 1
        ? new BoardPosition(rowIdx, 0)
        : new BoardPosition(rowIdx, playerPosition.ColumnIndex + 1);
    }

    private BoardPosition UpdatePlayerPositionSlideColumnUp(BoardPosition playerPosition, int columnIdx)
    {
      if (playerPosition.ColumnIndex != columnIdx)
      {
        return playerPosition;
      }

      return playerPosition.RowIndex == 0
        ? new BoardPosition(Board.GetHeight() - 1, columnIdx)
        : new BoardPosition(playerPosition.RowIndex - 1, columnIdx);
    }

    private BoardPosition UpdatePlayerPositionSlideColumnDown(BoardPosition playerPosition, int columnIdx)
    {
      if (playerPosition.ColumnIndex != columnIdx)
      {
        return playerPosition;
      }

      return playerPosition.RowIndex == Board.GetHeight() - 1
        ? new BoardPosition(0, columnIdx)
        : new BoardPosition(playerPosition.RowIndex + 1, columnIdx);
    }

    private void UpdateAllPlayerPositions(Func<BoardPosition, BoardPosition> positionUpdater)
    {
      PlayerQueue.ForEach(player => player.CurrentPosition = positionUpdater(player.CurrentPosition));
    }

    private static IBoard CopyBoard(IImmutableBoard board)
    {
      int height = board.GetHeight();
      int width = board.GetWidth();
      var matrix = new ITile[height, width];
      matrix.Fill((rowIdx, colIdx) =>
      {
        var boardPosition = new BoardPosition(rowIdx, colIdx);
        return board.GetTileAt(boardPosition);
      });
      return new Board(matrix);
    }

    private static void ValidatePlayers(IList<TPi> players, IImmutableBoard board, bool distinctHomes)
    {
      ValidateDistinctColors(players);
      ValidateCurrentPositionsAreOnTheBoard(players, board);
      ValidateHomePositionsAreOnTheBoard(players, board);
      ValidateHomePositionsAreNotMovable(players, board);
      if (distinctHomes)
      {
        ValidateDistinctHomes(players);
      }
    }

    private static void ValidateDistinctColors(IList<TPi> players)
    {
      ISet<Color> playerColors = players.Select(player => player.Color).ToHashSet();
      if (playerColors.Count < players.Count)
      {
        throw new ArgumentException("Player colors are not distinct.");
      }
    }

    private static void ValidateCurrentPositionsAreOnTheBoard(IList<TPi> players, IImmutableBoard board)
    {
      if (!players.All(player => IsInBoard(player.CurrentPosition, board)))
      {
        throw new ArgumentException("All player current positions must be on the board.");
      }
    }

    private static void ValidateHomePositionsAreOnTheBoard(IList<TPi> players, IImmutableBoard board)
    {
      if (!players.All(player => IsInBoard(player.HomePosition, board)))
      {
        throw new ArgumentException("All player home positions must be on the board.");
      }
    }

    private static void ValidateHomePositionsAreNotMovable(IList<TPi> players, IImmutableBoard board)
    {
      ISet<int> movableRows = board.SlidableRows.ToHashSet();
      ISet<int> movableColumns = board.SlidableColumns.ToHashSet();

      bool IsValid(BoardPosition home) =>
        !movableRows.Contains(home.RowIndex) && !movableColumns.Contains(home.ColumnIndex);

      if (players.Any(p => !IsValid(p.HomePosition)))
      {
        throw new ArgumentException("All player home positions must be on the immovable tiles.");
      }
    }

    private static void ValidateDistinctHomes(IList<TPi> players)
    {
      ISet<BoardPosition> playerHomes = players.Select(p => p.HomePosition).ToHashSet();
      if (playerHomes.Count < players.Count)
      {
        throw new ArgumentException("Player homes are not distinct.");
      }
    }

    private protected static bool IsInBoard(BoardPosition position, IImmutableBoard board)
    {
      return position.RowIndex >= 0 && position.RowIndex < board.GetHeight() && position.ColumnIndex >= 0 &&
             position.ColumnIndex < board.GetWidth();
    }
  }
}