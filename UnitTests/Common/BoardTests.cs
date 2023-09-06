using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Xunit;
using static UnitTests.Common.BoardUtils;

namespace UnitTests.Common
{
  public class BoardTests
  {
    #region BoardTests Examples

    private readonly IBoard _board;

    public BoardTests()
    {
      _board = CreateBoard();
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void TestConstructor()
    {
      var exception = Assert.Throws<ArgumentException>(() => { _ = new Board(new ITile[0, 0]); });
      Assert.Equal("The number of rows must be greater than or equal to 1.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _ = new Board(new ITile[1, 0]); });
      Assert.Equal("The number of columns must be greater than or equal to 1.", exception.Message);

      _ = new Board(new ITile[1, 1]);
      _ = new Board(new ITile[2, 2]);
      _ = new Board(new ITile[7, 7]);
      _ = new Board(new ITile[8, 8]);
      _ = new Board(new ITile[13, 13]);
      _ = new Board(new ITile[14, 14]);
    }

    #endregion

    #region IsReachable Tests

    [Fact]
    public void TestIsReachableInvalidSourcePosition()
    {
      var exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(-1, 3), new BoardPosition(4, 3));
      });
      Assert.Equal("Given board position 'source' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(7, 3), new BoardPosition(4, 3));
      });
      Assert.Equal("Given board position 'source' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(5, -1), new BoardPosition(4, 3));
      });
      Assert.Equal("Given board position 'source' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(5, 7), new BoardPosition(4, 3));
      });
      Assert.Equal("Given board position 'source' is invalid.", exception.Message);
    }

    [Fact]
    public void TestIsReachableInvalidDestinationPosition()
    {
      var exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(6, 2), new BoardPosition(-2, 4));
      });
      Assert.Equal("Given board position 'destination' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(2, 5), new BoardPosition(8, 1));
      });
      Assert.Equal("Given board position 'destination' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(6, 2), new BoardPosition(0, -3));
      });
      Assert.Equal("Given board position 'destination' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() =>
      {
        _board.IsReachable(new BoardPosition(2, 5), new BoardPosition(0, 7));
      });
      Assert.Equal("Given board position 'destination' is invalid.", exception.Message);
    }

    [Fact]
    public void TestIsReachableValidInputs()
    {
      Assert.True(_board.IsReachable(new BoardPosition(1, 1), new BoardPosition(0, 1)));
      Assert.True(_board.IsReachable(new BoardPosition(1, 1), new BoardPosition(1, 0)));
      Assert.True(_board.IsReachable(new BoardPosition(1, 1), new BoardPosition(2, 1)));
      Assert.True(_board.IsReachable(new BoardPosition(0, 0), new BoardPosition(0, 1)));

      Assert.False(_board.IsReachable(new BoardPosition(1, 2), new BoardPosition(0, 2)));
      Assert.False(_board.IsReachable(new BoardPosition(2, 2), new BoardPosition(2, 1)));
      Assert.False(_board.IsReachable(new BoardPosition(1, 6), new BoardPosition(2, 6)));
      Assert.False(_board.IsReachable(new BoardPosition(5, 3), new BoardPosition(5, 4)));

      Assert.True(_board.IsReachable(new BoardPosition(1, 4), new BoardPosition(6, 5)));
      Assert.True(_board.IsReachable(new BoardPosition(0, 0), new BoardPosition(6, 1)));
      Assert.True(_board.IsReachable(new BoardPosition(6, 4), new BoardPosition(2, 4)));

      Assert.False(_board.IsReachable(new BoardPosition(5, 5), new BoardPosition(0, 6)));
      Assert.False(_board.IsReachable(new BoardPosition(0, 0), new BoardPosition(6, 6)));
      Assert.False(_board.IsReachable(new BoardPosition(6, 0), new BoardPosition(0, 6)));

      // a tile is reachable to itself
      Assert.True(_board.IsReachable(new BoardPosition(3, 4), new BoardPosition(3, 4)));
      Assert.True(_board.IsReachable(new BoardPosition(4, 3), new BoardPosition(4, 3)));
      Assert.True(_board.IsReachable(new BoardPosition(1, 6), new BoardPosition(1, 6)));
    }

    #endregion

    #region GetAllReachablePositions Tests

    [Fact]
    public void TestGetAllReachablePositions()
    {
      IList<BoardPosition> actual = _board.GetAllReachablePositions(new BoardPosition(0, 0)).ToList();
      ISet<BoardPosition> expected = new HashSet<BoardPosition>
      {
        new(0, 0),
        new(0, 1),
        new(1, 0),
        new(1, 1),
        new(2, 0),
        new(2, 1),
        new(3, 0),
        new(3, 1),
        new(3, 2),
        new(4, 0),
        new(4, 1),
        new(4, 2),
        new(5, 0),
        new(5, 1),
        new(5, 2),
        new(6, 1),
      };

      Assert.Equal(expected, actual);
    }

    #endregion

    #region SlideRowToLeft Tests

    [Fact]
    public void TestSlideRowToLeftInvalidRowIndex()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToLeft(tileToInsert, -1); });
      Assert.Equal("Row index is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToLeft(tileToInsert, 7); });
      Assert.Equal("Row index is invalid.", exception.Message);
    }

    [Fact]
    public void TestSlideRowToLeftRowNotSlidable()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToLeft(tileToInsert, 1); });
      Assert.Equal("Only even row indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToLeft(tileToInsert, 3); });
      Assert.Equal("Only even row indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToLeft(tileToInsert, 5); });
      Assert.Equal("Only even row indices are slidable.", exception.Message);
    }

    [Fact]
    public void TestSlideRowToLeft()
    {
      ITile tile0 = _board.GetTileAt(new BoardPosition(0, 0));
      ITile tile1 = _board.GetTileAt(new BoardPosition(0, 1));
      ITile tile2 = _board.GetTileAt(new BoardPosition(0, 2));
      ITile tile3 = _board.GetTileAt(new BoardPosition(0, 3));
      ITile tile4 = _board.GetTileAt(new BoardPosition(0, 4));
      ITile tile5 = _board.GetTileAt(new BoardPosition(0, 5));
      ITile tile6 = _board.GetTileAt(new BoardPosition(0, 6));

      ITile tileToInsert = new Tile(false, true, false, true, new Treasure(Gem.Sunstone, Gem.RedDiamond));
      ITile popOutTile = _board.SlideRowToLeft(tileToInsert, 0);

      Assert.Equal(popOutTile, tile0);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 0)), tile1);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 1)), tile2);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 2)), tile3);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 3)), tile4);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 4)), tile5);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 5)), tile6);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 6)), tileToInsert);
    }

    #endregion

    #region SlideRowToRight Tests

    [Fact]
    public void TestSlideRowToRightInvalidRowIndex()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToRight(tileToInsert, -1); });
      Assert.Equal("Row index is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToRight(tileToInsert, 7); });
      Assert.Equal("Row index is invalid.", exception.Message);
    }

    [Fact]
    public void TestSlideRowToRightRowNotSlidable()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToRight(tileToInsert, 1); });
      Assert.Equal("Only even row indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToRight(tileToInsert, 3); });
      Assert.Equal("Only even row indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideRowToRight(tileToInsert, 5); });
      Assert.Equal("Only even row indices are slidable.", exception.Message);
    }

    [Fact]
    public void TestSlideRowToRight()
    {
      ITile tile0 = _board.GetTileAt(new BoardPosition(4, 0));
      ITile tile1 = _board.GetTileAt(new BoardPosition(4, 1));
      ITile tile2 = _board.GetTileAt(new BoardPosition(4, 2));
      ITile tile3 = _board.GetTileAt(new BoardPosition(4, 3));
      ITile tile4 = _board.GetTileAt(new BoardPosition(4, 4));
      ITile tile5 = _board.GetTileAt(new BoardPosition(4, 5));
      ITile tile6 = _board.GetTileAt(new BoardPosition(4, 6));

      ITile tileToInsert = new Tile(false, true, false, true, new Treasure(Gem.Sunstone, Gem.RedDiamond));
      ITile popOutTile = _board.SlideRowToRight(tileToInsert, 4);

      Assert.Equal(popOutTile, tile6);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 6)), tile5);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 5)), tile4);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 4)), tile3);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 3)), tile2);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 2)), tile1);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 1)), tile0);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 0)), tileToInsert);
    }

    #endregion

    #region SlideColumnToUp Tests

    [Fact]
    public void TestSlideColumnToUpInvalidColumnIndex()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToUp(tileToInsert, -1); });
      Assert.Equal("Column index is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToUp(tileToInsert, 7); });
      Assert.Equal("Column index is invalid.", exception.Message);
    }

    [Fact]
    public void TestSlideColumnToUpColumnNotSlidable()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToUp(tileToInsert, 1); });
      Assert.Equal("Only even column indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToUp(tileToInsert, 3); });
      Assert.Equal("Only even column indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToUp(tileToInsert, 5); });
      Assert.Equal("Only even column indices are slidable.", exception.Message);
    }

    [Fact]
    public void TestSlideColumnToUp()
    {
      ITile tile0 = _board.GetTileAt(new BoardPosition(0, 6));
      ITile tile1 = _board.GetTileAt(new BoardPosition(1, 6));
      ITile tile2 = _board.GetTileAt(new BoardPosition(2, 6));
      ITile tile3 = _board.GetTileAt(new BoardPosition(3, 6));
      ITile tile4 = _board.GetTileAt(new BoardPosition(4, 6));
      ITile tile5 = _board.GetTileAt(new BoardPosition(5, 6));
      ITile tile6 = _board.GetTileAt(new BoardPosition(6, 6));

      ITile tileToInsert = new Tile(false, true, false, true, new Treasure(Gem.Sunstone, Gem.RedDiamond));
      ITile popOutTile = _board.SlideColumnToUp(tileToInsert, 6);

      Assert.Equal(popOutTile, tile0);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 6)), tile1);
      Assert.Equal(_board.GetTileAt(new BoardPosition(1, 6)), tile2);
      Assert.Equal(_board.GetTileAt(new BoardPosition(2, 6)), tile3);
      Assert.Equal(_board.GetTileAt(new BoardPosition(3, 6)), tile4);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 6)), tile5);
      Assert.Equal(_board.GetTileAt(new BoardPosition(5, 6)), tile6);
      Assert.Equal(_board.GetTileAt(new BoardPosition(6, 6)), tileToInsert);
    }

    #endregion

    #region SlideRowToDown Tests

    [Fact]
    public void TestSlideColumnToDownInvalidColumnIndex()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToDown(tileToInsert, -1); });
      Assert.Equal("Column index is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToDown(tileToInsert, 7); });
      Assert.Equal("Column index is invalid.", exception.Message);
    }

    [Fact]
    public void TestSlideColumnToDownColumnNotSlidable()
    {
      var tileToInsert = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Goldstone));

      var exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToDown(tileToInsert, 1); });
      Assert.Equal("Only even column indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToDown(tileToInsert, 3); });
      Assert.Equal("Only even column indices are slidable.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.SlideColumnToDown(tileToInsert, 5); });
      Assert.Equal("Only even column indices are slidable.", exception.Message);
    }

    [Fact]
    public void TestSlideColumnToDown()
    {
      ITile tile0 = _board.GetTileAt(new BoardPosition(0, 2));
      ITile tile1 = _board.GetTileAt(new BoardPosition(1, 2));
      ITile tile2 = _board.GetTileAt(new BoardPosition(2, 2));
      ITile tile3 = _board.GetTileAt(new BoardPosition(3, 2));
      ITile tile4 = _board.GetTileAt(new BoardPosition(4, 2));
      ITile tile5 = _board.GetTileAt(new BoardPosition(5, 2));
      ITile tile6 = _board.GetTileAt(new BoardPosition(6, 2));

      ITile tileToInsert = new Tile(false, true, false, true, new Treasure(Gem.Sunstone, Gem.RedDiamond));
      ITile popOutTile = _board.SlideColumnToDown(tileToInsert, 2);

      Assert.Equal(popOutTile, tile6);
      Assert.Equal(_board.GetTileAt(new BoardPosition(6, 2)), tile5);
      Assert.Equal(_board.GetTileAt(new BoardPosition(5, 2)), tile4);
      Assert.Equal(_board.GetTileAt(new BoardPosition(4, 2)), tile3);
      Assert.Equal(_board.GetTileAt(new BoardPosition(3, 2)), tile2);
      Assert.Equal(_board.GetTileAt(new BoardPosition(2, 2)), tile1);
      Assert.Equal(_board.GetTileAt(new BoardPosition(1, 2)), tile0);
      Assert.Equal(_board.GetTileAt(new BoardPosition(0, 2)), tileToInsert);
    }

    #endregion

    #region GetWidth Tests

    [Fact]
    public void TestGetWidth()
    {
      Assert.Equal(7, _board.GetWidth());
    }

    #endregion

    #region GetHeight Tests

    [Fact]
    public void TestGetHeight()
    {
      Assert.Equal(7, _board.GetHeight());
    }

    #endregion

    #region GetTileAt Tests

    [Fact]
    public void TestGetTileAtInvalidPosition()
    {
      var exception = Assert.Throws<ArgumentException>(() => { _board.GetTileAt(new BoardPosition(9, 1)); });
      Assert.Equal("Given board position 'position' is invalid.", exception.Message);

      exception = Assert.Throws<ArgumentException>(() => { _board.GetTileAt(new BoardPosition(4, -1)); });
      Assert.Equal("Given board position 'position' is invalid.", exception.Message);
    }

    [Fact]
    public void TestGetTileAt()
    {
      ITile tile1 = new Tile(false, true, false, true, new Treasure(Gem.YellowJasper, Gem.Carnelian));
      ITile tile2 = new Tile(true, false, true, false, new Treasure(Gem.Aquamarine, Gem.Carnelian));
      ITile tile3 = new Tile(true, true, true, true, new Treasure(Gem.Moonstone, Gem.BlackOnyx));
      ITile tile4 = new Tile(false, true, true, false, new Treasure(Gem.Moonstone, Gem.BlackOnyx));

      IBoard board = new Board(new[,]
      {
        {tile1, tile2, tile1},
        {tile3, tile4, tile1},
        {tile1, tile1, tile1}
      });

      Assert.Equal(tile1, board.GetTileAt(new BoardPosition(0, 0)));
      Assert.Equal(tile2, board.GetTileAt(new BoardPosition(0, 1)));
      Assert.Equal(tile3, board.GetTileAt(new BoardPosition(1, 0)));
      Assert.Equal(tile4, board.GetTileAt(new BoardPosition(1, 1)));
    }

    #endregion
  }
}