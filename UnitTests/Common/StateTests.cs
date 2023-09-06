using System;
using System.Collections.Generic;
using Common;
using Xunit;
using static UnitTests.Common.BoardUtils;

namespace UnitTests.Common
{
  public class StateTests
  {
    [Fact]
    public void TestConstructorDistinctColors()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(1, 1), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(1, 3), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(1, 5), new BoardPosition(1, 5), treasure),
        new PlayerInfo(Color.Red, new BoardPosition(3, 1), new BoardPosition(3, 1), treasure),
      };

      var exception = Assert.Throws<ArgumentException>(() => new RefereeState(players, board, spareTile));
      Assert.Equal("Player colors are not distinct.", exception.Message);
    }

    [Fact]
    public void TestConstructorCurrentPositionNotOnBoard()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(1, 1), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(1, 3), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(1, 5), new BoardPosition(1, 7), treasure),
        new PlayerInfo(Color.Purple, new BoardPosition(3, 1), new BoardPosition(3, 1), treasure),
      };

      var exception = Assert.Throws<ArgumentException>(() => new RefereeState(players, board, spareTile));
      Assert.Equal("All player current positions must be on the board.", exception.Message);
    }

    [Fact]
    public void TestConstructorHomePositionNotOnBoard()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(1, 1), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(1, 3), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(1, 7), new BoardPosition(1, 5), treasure),
        new PlayerInfo(Color.Purple, new BoardPosition(3, 1), new BoardPosition(3, 1), treasure),
      };

      var exception = Assert.Throws<ArgumentException>(() => new RefereeState(players, board, spareTile));
      Assert.Equal("All player home positions must be on the board.", exception.Message);
    }

    [Fact]
    public void TestConstructorHomePositionOnMovableTile()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(1, 1), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(1, 3), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(2, 5), new BoardPosition(1, 5), treasure),
        new PlayerInfo(Color.Purple, new BoardPosition(3, 1), new BoardPosition(3, 1), treasure),
      };

      var exception = Assert.Throws<ArgumentException>(() => new RefereeState(players, board, spareTile));
      Assert.Equal("All player home positions must be on the immovable tiles.", exception.Message);
    }

    [Fact]
    public void TestConstructorDistinctHomes()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(1, 1), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(1, 3), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(1, 5), new BoardPosition(1, 5), treasure),
        new PlayerInfo(Color.Purple, new BoardPosition(1, 1), new BoardPosition(3, 1), treasure),
      };

      var exception = Assert.Throws<ArgumentException>(() => new RefereeState(players, board, spareTile));
      Assert.Equal("Player homes are not distinct.", exception.Message);
    }

    [Fact]
    public void TestConstructorSuccessful()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(1, 1), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(1, 3), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(1, 5), new BoardPosition(1, 5), treasure),
        new PlayerInfo(Color.Purple, new BoardPosition(3, 1), new BoardPosition(3, 1), treasure),
        new PlayerInfo(Color.Orange, new BoardPosition(3, 3), new BoardPosition(3, 3), treasure),
        new PlayerInfo(Color.Yellow, new BoardPosition(3, 5), new BoardPosition(3, 5), treasure),
        new PlayerInfo(Color.Pink, new BoardPosition(5, 1), new BoardPosition(5, 1), treasure),
        new PlayerInfo(Color.White, new BoardPosition(5, 3), new BoardPosition(5, 3), treasure),
        new PlayerInfo(Color.Black, new BoardPosition(5, 5), new BoardPosition(5, 5), treasure),
      };

      _ = new RefereeState(players, board, spareTile);
    }
  }
}