using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;
using Xunit;
using static UnitTests.Common.BoardUtils;

namespace UnitTests.Common
{
  public class RefereeStateTests
  {
    private readonly IRefereeState _state1;
    private readonly IRefereeState _state2;
    private readonly IRefereeState _state3;

    public RefereeStateTests()
    {
      _state1 = CreateState1();
      _state2 = CreateState2();
      _state3 = CreateState3();
    }

    [Fact]
    public void TestRemoveActivePlayer()
    {
      Assert.Equal(Color.Red, _state1.ActivePlayer.Color);
      Assert.Equal(5, _state1.AllPlayers.Count());

      _state1.RemoveActivePlayer();

      Assert.Equal(Color.Green, _state1.ActivePlayer.Color);
      Assert.Equal(4, _state1.AllPlayers.Count());

      _state1.NextTurn();
      _state1.NextTurn();
      _state1.NextTurn();
      _state1.RemoveActivePlayer();

      Assert.Equal(Color.Green, _state1.ActivePlayer.Color);
      Assert.Equal(3, _state1.AllPlayers.Count());

      _state1.RemoveActivePlayer();

      Assert.Equal(Color.Blue, _state1.ActivePlayer.Color);
      Assert.Equal(2, _state1.AllPlayers.Count());

      _state1.RemoveActivePlayer();

      Assert.Equal(Color.Pink, _state1.ActivePlayer.Color);
      Assert.Single(_state1.AllPlayers);

      _state1.RemoveActivePlayer();
      Assert.Empty(_state1.AllPlayers);

      Assert.Throws<InvalidOperationException>(() => _state1.RemoveActivePlayer());
    }

    [Fact]
    public void TestRemovePlayer()
    {
      var players = _state1.AllPlayers.ToList();
      Assert.Equal(5, _state1.AllPlayers.Count());
      _state1.RemovePlayer(players[2]);
      Assert.Equal(4, _state1.AllPlayers.Count());
      Assert.Equal(Color.Red, _state1.ActivePlayer.Color);
      _state1.NextTurn();
      Assert.Equal(Color.Green, _state1.ActivePlayer.Color);
      _state1.RemovePlayer(players[4]);
      Assert.Equal(3, _state1.AllPlayers.Count());
      Assert.Equal(Color.Green, _state1.ActivePlayer.Color);
      _state1.NextTurn();
      Assert.Equal(Color.Pink, _state1.ActivePlayer.Color);
      _state1.RemovePlayer(players[3]);
      Assert.Equal(2, _state1.AllPlayers.Count());
      Assert.Equal(Color.Red, _state1.ActivePlayer.Color);
    }

    [Fact]
    public void TestRotateSpareTileZero()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      var spareTile = new Tile(true, true, false, false, treasure);
      Assert.Equal(spareTile, _state1.SpareTile);

      _state1.RotateSpareTile(Rotation.Zero);
      Assert.Equal(spareTile, _state1.SpareTile);
    }

    [Fact]
    public void TestRotateSpareTileNinety()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      var spareTile = new Tile(true, false, false, true, treasure);

      _state1.RotateSpareTile(Rotation.Ninety);
      Assert.Equal(spareTile, _state1.SpareTile);
    }

    [Fact]
    public void TestRotateSpareTileOneHundredEighty()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      var spareTile = new Tile(false, false, true, true, treasure);

      _state1.RotateSpareTile(Rotation.OneHundredEighty);
      Assert.Equal(spareTile, _state1.SpareTile);
    }

    [Fact]
    public void TestRotateSpareTileTwoHundredSeventy()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      var spareTile = new Tile(false, true, true, false, treasure);

      _state1.RotateSpareTile(Rotation.TwoHundredSeventy);
      Assert.Equal(spareTile, _state1.SpareTile);
    }

    [Fact]
    public void TestPerformSlideRowLeft()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      purplePlayer.CurrentPosition = new BoardPosition(0, 0);

      var orangePlayer = _state2.GetPlayer(Color.Orange);
      orangePlayer.CurrentPosition = new BoardPosition(0, 4);

      var pinkPlayer = _state2.GetPlayer(Color.Pink);
      pinkPlayer.CurrentPosition = new BoardPosition(1, 5);

      var spareTile = _state2.SpareTile;
      var futureSpareTile = _state2.Board.GetTileAt(new BoardPosition(0, 0));
      Assert.True(_state2.LastSlideAction.IsNone);

      _state2.PerformSlide(new SlideAction(SlideType.SlideRowLeft, 0));

      Assert.Equal(new BoardPosition(0, 6), purplePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(0, 3), orangePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(1, 5), pinkPlayer.CurrentPosition);

      Assert.Equal(spareTile, _state2.Board.GetTileAt(new BoardPosition(0, 6)));
      Assert.Equal(futureSpareTile, _state2.SpareTile);
      Assert.True(
        _state2.LastSlideAction
          .Some(last => last.Equals(new SlideAction(SlideType.SlideRowLeft, 0)))
          .None(() => false)
      );
    }

    [Fact]
    public void TestPerformSlideRowRight()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      purplePlayer.CurrentPosition = new BoardPosition(6, 3);

      var orangePlayer = _state2.GetPlayer(Color.Orange);
      orangePlayer.CurrentPosition = new BoardPosition(4, 6);

      var pinkPlayer = _state2.GetPlayer(Color.Pink);
      pinkPlayer.CurrentPosition = new BoardPosition(4, 1);

      var spareTile = _state2.SpareTile;
      var futureSpareTile = _state2.Board.GetTileAt(new BoardPosition(4, 6));
      Assert.True(_state2.LastSlideAction.IsNone);

      _state2.PerformSlide(new SlideAction(SlideType.SlideRowRight, 4));

      Assert.Equal(new BoardPosition(6, 3), purplePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(4, 0), orangePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(4, 2), pinkPlayer.CurrentPosition);

      Assert.Equal(spareTile, _state2.Board.GetTileAt(new BoardPosition(4, 0)));
      Assert.Equal(futureSpareTile, _state2.SpareTile);
      Assert.True(
        _state2.LastSlideAction
          .Some(last => last.Equals(new SlideAction(SlideType.SlideRowRight, 4)))
          .None(() => false)
      );
    }

    [Fact]
    public void TestPerformSlideColumnUp()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      purplePlayer.CurrentPosition = new BoardPosition(6, 2);

      var orangePlayer = _state2.GetPlayer(Color.Orange);
      orangePlayer.CurrentPosition = new BoardPosition(2, 3);

      var pinkPlayer = _state2.GetPlayer(Color.Pink);
      pinkPlayer.CurrentPosition = new BoardPosition(0, 2);

      var spareTile = _state2.SpareTile;
      var futureSpareTile = _state2.Board.GetTileAt(new BoardPosition(0, 2));
      Assert.True(_state2.LastSlideAction.IsNone);

      _state2.PerformSlide(new SlideAction(SlideType.SlideColumnUp, 2));

      Assert.Equal(new BoardPosition(5, 2), purplePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(2, 3), orangePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(6, 2), pinkPlayer.CurrentPosition);

      Assert.Equal(spareTile, _state2.Board.GetTileAt(new BoardPosition(6, 2)));
      Assert.Equal(futureSpareTile, _state2.SpareTile);
      Assert.True(
        _state2.LastSlideAction
          .Some(last => last.Equals(new SlideAction(SlideType.SlideColumnUp, 2)))
          .None(() => false)
      );
    }

    [Fact]
    public void TestPerformSlideColumnDown()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      purplePlayer.CurrentPosition = new BoardPosition(3, 6);

      var orangePlayer = _state2.GetPlayer(Color.Orange);
      orangePlayer.CurrentPosition = new BoardPosition(3, 3);

      var pinkPlayer = _state2.GetPlayer(Color.Pink);
      pinkPlayer.CurrentPosition = new BoardPosition(6, 6);

      var spareTile = _state2.SpareTile;
      var futureSpareTile = _state2.Board.GetTileAt(new BoardPosition(6, 6));
      Assert.True(_state2.LastSlideAction.IsNone);

      _state2.PerformSlide(new SlideAction(SlideType.SlideColumnDown, 6));

      Assert.Equal(new BoardPosition(4, 6), purplePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(3, 3), orangePlayer.CurrentPosition);
      Assert.Equal(new BoardPosition(0, 6), pinkPlayer.CurrentPosition);

      Assert.Equal(spareTile, _state2.Board.GetTileAt(new BoardPosition(0, 6)));
      Assert.Equal(futureSpareTile, _state2.SpareTile);
      Assert.True(
        _state2.LastSlideAction
          .Some(last => last.Equals(new SlideAction(SlideType.SlideColumnDown, 6)))
          .None(() => false)
      );
    }

    [Fact]
    public void TestCanActivePlayerReach()
    {
      Assert.True(_state2.CanActivePlayerReach(new BoardPosition(1, 1)));
      Assert.True(_state2.CanActivePlayerReach(new BoardPosition(5, 1)));
      Assert.True(_state2.CanActivePlayerReach(new BoardPosition(3, 1)));

      Assert.False(_state2.CanActivePlayerReach(new BoardPosition(1, 2)));
      Assert.False(_state2.CanActivePlayerReach(new BoardPosition(2, 6)));
      Assert.False(_state2.CanActivePlayerReach(new BoardPosition(5, 4)));
    }

    [Fact]
    public void TestAllReachablePositionsByActivePlayer()
    {
      ISet<BoardPosition> expectedSet = new System.Collections.Generic.HashSet<BoardPosition>
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

      ISet<BoardPosition> actualSet = _state2.AllReachablePositionsByActivePlayer.ToHashSet();
      Assert.Equal(expectedSet, actualSet);
    }

    [Fact]
    public void TestNextTurn()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      var orangePlayer = _state2.GetPlayer(Color.Orange);
      var pinkPlayer = _state2.GetPlayer(Color.Pink);

      Assert.Equal(purplePlayer, _state2.ActivePlayer);
      _state2.NextTurn();
      Assert.Equal(orangePlayer, _state2.ActivePlayer);
      _state2.NextTurn();
      Assert.Equal(pinkPlayer, _state2.ActivePlayer);
      _state2.NextTurn();
      Assert.Equal(purplePlayer, _state2.ActivePlayer);
    }

    [Fact]
    public void TestHasActivePlayerReachedTreasure()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      var orangePlayer = _state2.GetPlayer(Color.Orange);
      var pinkPlayer = _state2.GetPlayer(Color.Pink);

      Assert.False(_state2.HasActivePlayerReachedTreasure());
      _state2.NextTurn();
      Assert.False(_state2.HasActivePlayerReachedTreasure());
      _state2.NextTurn();
      Assert.False(_state2.HasActivePlayerReachedTreasure());
      _state2.NextTurn();

      purplePlayer.CurrentPosition = new BoardPosition(5, 5);
      orangePlayer.CurrentPosition = new BoardPosition(5, 3);
      pinkPlayer.CurrentPosition = new BoardPosition(5, 1);

      Assert.True(_state2.HasActivePlayerReachedTreasure());
      _state2.NextTurn();
      Assert.True(_state2.HasActivePlayerReachedTreasure());
      _state2.NextTurn();
      Assert.True(_state2.HasActivePlayerReachedTreasure());
    }

    [Fact]
    public void TestHasActivePlayerReachedHome()
    {
      var purplePlayer = _state2.GetPlayer(Color.Purple);
      var orangePlayer = _state2.GetPlayer(Color.Orange);
      var pinkPlayer = _state2.GetPlayer(Color.Pink);

      purplePlayer.CurrentPosition = new BoardPosition(0, 0);
      orangePlayer.CurrentPosition = new BoardPosition(5, 3);
      pinkPlayer.CurrentPosition = new BoardPosition(6, 2);

      Assert.False(_state2.HasActivePlayerReachedHome());
      _state2.NextTurn();
      Assert.False(_state2.HasActivePlayerReachedHome());
      _state2.NextTurn();
      Assert.False(_state2.HasActivePlayerReachedHome());
      _state2.NextTurn();

      purplePlayer.CurrentPosition = new BoardPosition(1, 1);
      orangePlayer.CurrentPosition = new BoardPosition(1, 3);
      pinkPlayer.CurrentPosition = new BoardPosition(1, 5);

      Assert.True(_state2.HasActivePlayerReachedHome());
      _state2.NextTurn();
      Assert.True(_state2.HasActivePlayerReachedHome());
      _state2.NextTurn();
      Assert.True(_state2.HasActivePlayerReachedHome());
    }

    [Fact]
    public void TestMoveActivePlayer()
    {
      Assert.Equal(new BoardPosition(1, 1), _state2.ActivePlayer.CurrentPosition);
      _state2.NextTurn();
      Assert.Equal(new BoardPosition(1, 3), _state2.ActivePlayer.CurrentPosition);
      _state2.NextTurn();
      Assert.Equal(new BoardPosition(1, 5), _state2.ActivePlayer.CurrentPosition);
      _state2.NextTurn();

      _state2.MoveActivePlayer(new BoardPosition(5, 2));
      _state2.NextTurn();
      _state2.MoveActivePlayer(new BoardPosition(3, 6));
      _state2.NextTurn();
      _state2.MoveActivePlayer(new BoardPosition(0, 4));
      _state2.NextTurn();

      Assert.Equal(new BoardPosition(5, 2), _state2.ActivePlayer.CurrentPosition);
      _state2.NextTurn();
      Assert.Equal(new BoardPosition(3, 6), _state2.ActivePlayer.CurrentPosition);
      _state2.NextTurn();
      Assert.Equal(new BoardPosition(0, 4), _state2.ActivePlayer.CurrentPosition);
    }

    [Fact]
    public void TestPopNextTreasure()
    {
      Assert.Equal(4, _state3.NumberOfTreasures);
      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Alexandrite), _state3.NextTreasure);

      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Alexandrite), _state3.PopNextTreasure());
      Assert.Equal(3, _state3.NumberOfTreasures);
      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Amethyst), _state3.NextTreasure);

      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Amethyst), _state3.PopNextTreasure());
      Assert.Equal(2, _state3.NumberOfTreasures);
      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Ametrine), _state3.NextTreasure);

      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Ametrine), _state3.PopNextTreasure());
      Assert.Equal(1, _state3.NumberOfTreasures);
      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Ammolite), _state3.NextTreasure);

      Assert.Equal(new Treasure(Gem.AlexandritePearShape, Gem.Ammolite), _state3.PopNextTreasure());
      Assert.Equal(0, _state3.NumberOfTreasures);
      Assert.Throws<InvalidOperationException>(() => _state3.NextTreasure);
      Assert.Throws<InvalidOperationException>(() => _state3.PopNextTreasure());
    }

    #region Test Helpers

    private static IRefereeState CreateState1()
    {
      ITreasure treasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, treasure);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Red, new BoardPosition(1, 1), new BoardPosition(0, 0), treasure),
        new PlayerInfo(Color.Green, new BoardPosition(1, 3), new BoardPosition(0, 0), treasure),
        new PlayerInfo(Color.Blue, new BoardPosition(1, 5), new BoardPosition(0, 0), treasure),
        new PlayerInfo(Color.Pink, new BoardPosition(3, 1), new BoardPosition(0, 0), treasure),
        new PlayerInfo(Color.Orange, new BoardPosition(3, 3), new BoardPosition(0, 0), treasure),
      };

      return new RefereeState(players, board, spareTile);
    }

    private static IRefereeState CreateState2()
    {
      ITreasure spareTreasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, spareTreasure);

      ITreasure purpleTreasure = new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique);
      ITreasure orangeTreasure = new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate);
      ITreasure pinkTreasure = new Treasure(Gem.AlexandritePearShape, Gem.Goldstone);
      IBoard board = CreateBoard();
      var players = new List<IPlayerInfo>
      {
        new PlayerInfo(Color.Purple, new BoardPosition(1, 1), new BoardPosition(1, 1), purpleTreasure),
        new PlayerInfo(Color.Orange, new BoardPosition(1, 3), new BoardPosition(1, 3), orangeTreasure),
        new PlayerInfo(Color.Pink, new BoardPosition(1, 5), new BoardPosition(1, 5), pinkTreasure),
      };

      return new RefereeState(players, board, spareTile);
    }

    private static IRefereeState CreateState3()
    {
      ITreasure spareTreasure = new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape);
      ITile spareTile = new Tile(true, true, false, false, spareTreasure);
      var treasures = new List<ITreasure>
      {
        new Treasure(Gem.AlexandritePearShape, Gem.Alexandrite),
        new Treasure(Gem.AlexandritePearShape, Gem.Amethyst),
        new Treasure(Gem.AlexandritePearShape, Gem.Ametrine),
        new Treasure(Gem.AlexandritePearShape, Gem.Ammolite),
      };
      return new RefereeState(Array.Empty<IPlayerInfo>(), CreateBoard(), spareTile, Option<ISlideAction>.None,
        treasures);
    }

    #endregion
  }
}