using System;
using System.Collections.Generic;
using Common;
using LanguageExt;
using Players;
using Xunit;

namespace UnitTests.Players
{
  public class PlayerTests
  {
    private readonly IPlayer _player;
    private readonly IBoard _board;

    public PlayerTests()
    {
      var tiles = new ITile[7, 7];
      tiles.Fill((_, _) =>
      {
        ITreasure treasure = new Treasure(Gem.Alexandrite, Gem.Alexandrite);
        ITile tile = new Tile(true, true, true, true, treasure);
        return tile;
      });
      _board = new Board(tiles);

      IBoardBuilder boardBuilder = new MockBoardBuilder(_board);

      _player = new Player("Can", new PassStrategy(), boardBuilder);
    }

    [Fact]
    public void TestName()
    {
      Assert.Equal("Can", _player.Name);
    }

    [Fact]
    public void TestProposeBoard()
    {
      Assert.Equal(_board, _player.ProposeBoard(7, 7));
    }

    [Fact]
    public void TestSetup()
    {
      Assert.Equal(Acknowledge.Value, _player.Setup(new MockState(), new BoardPosition(0, 0)));
    }
    
    [Fact]
    public void TestTakeTurn()
    {
      Assert.Equal(Either<IMove, Pass>.Right(Pass.Value), _player.TakeTurn(new MockState()));
    }
    
    [Fact]
    public void TestWon()
    {
      Assert.Equal(Acknowledge.Value, _player.Won(true));
    }
  }

  public sealed class MockBoardBuilder : IBoardBuilder
  {
    private readonly IBoard _board;

    public MockBoardBuilder(IBoard board)
    {
      _board = board;
    }

    public IBoard BuildBoard(int height, int width)
    {
      return _board;
    }
  }

  public sealed class MockState : IPlayerState
  {
    public IImmutableBoard Board => throw new NotImplementedException();
    public ITile SpareTile => throw new NotImplementedException();
    public Option<ISlideAction> LastSlideAction => throw new NotImplementedException();
    public IImmutablePublicPlayerInfo ActivePlayer => throw new NotImplementedException();

    public bool CanActivePlayerReach(BoardPosition destination)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<BoardPosition> AllReachablePositionsByActivePlayer => throw new NotImplementedException();
    public IEnumerable<IImmutablePublicPlayerInfo> AllPlayers => throw new NotImplementedException();

    public IImmutablePublicPlayerInfo GetPlayer(Color color)
    {
      throw new NotImplementedException();
    }

    public IPlayerState RotateSpareTile(Rotation rotation)
    {
      return this;
    }

    public IPlayerState PerformSlide(ISlideAction slideAction)
    {
      return this;
    }
  }
}