using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Common
{
  public sealed class RefereeState : AState<IPlayerInfo>, IRefereeState
  {
    private readonly Queue<ITreasure> _treasures;

    public RefereeState(IEnumerable<IPlayerInfo> players, IImmutableBoard board, ITile spareTile, bool distinctHomes = true) :
      this(CopyPlayers(players), board, spareTile, Option<ISlideAction>.None,distinctHomes)
    {
    }

    private RefereeState(IEnumerable<IPlayerInfo> players, IImmutableBoard board, ITile spareTile,
      Option<ISlideAction> lastSlideAction, bool distinctHomes = true) : this(CopyPlayers(players), board, spareTile,
      lastSlideAction, Array.Empty<ITreasure>(), distinctHomes)
    {
    }

    public RefereeState(IEnumerable<IPlayerInfo> players, IImmutableBoard board, ITile spareTile,
      Option<ISlideAction> lastSlideAction, IList<ITreasure> treasures, bool distinctHomes = true) : base(
      CopyPlayers(players), board, spareTile, lastSlideAction, distinctHomes)
    {
      if (!treasures.All(board.HasTreasure))
      {
        throw new ArgumentException("The board must contain all extra treasures.");
      }

      _treasures = new Queue<ITreasure>(treasures);
    }

    void IRefereeState.RotateSpareTile(Rotation rotation)
    {
      RotateSpareTile(rotation);
    }

    void IRefereeState.PerformSlide(ISlideAction slideAction)
    {
      PerformSlide(slideAction);
    }

    public IPlayerInfo RemoveActivePlayer()
    {
      if (PlayerQueue.Count == 0)
      {
        throw new InvalidOperationException("There are no players.");
      }

      return PlayerQueue.Dequeue();
    }

    public void RemovePlayer(IPlayerInfo player)
    {
      PlayerQueue = new Queue<IPlayerInfo>(
        PlayerQueue.Where(p => !p.Equals(player))
      );
    }

    public void NextTurn()
    {
      var playerInfo = PlayerQueue.Dequeue();
      PlayerQueue.Enqueue(playerInfo);
    }

    public bool HasActivePlayerReachedTreasure()
    {
      ITile tile = Board.GetTileAt(ActivePlayer.CurrentPosition);
      return tile.Treasure.Equals(ActivePlayer.CurrentlyAssignedTreasure);
    }

    public bool HasActivePlayerReachedHome() => ActivePlayer.CurrentPosition.Equals(ActivePlayer.HomePosition);

    public void MoveActivePlayer(BoardPosition destination)
    {
      if (!IsInBoard(destination, Board))
      {
        throw new ArgumentException("New player position is not on the board");
      }

      ActivePlayer.CurrentPosition = destination;
    }

    public IRefereeState Copy() => new RefereeState(AllPlayers, Board, SpareTile, LastSlideAction, false);

    public IPlayerState ToPlayerState() => new PlayerState(AllPlayers, Board, SpareTile, LastSlideAction, false);

    public new IPlayerInfo ActivePlayer => PlayerQueue.Peek();

    public new IEnumerable<IPlayerInfo> AllPlayers => PlayerQueue;

    public new IPlayerInfo GetPlayer(Color color) => AllPlayers
      .Find(player => player.Color.Equals(color))
      .Some(player => player)
      .None(() => throw new ArgumentException("Player with the given color does not exist."));

    public int NumberOfTreasures => _treasures.Count;

    public ITreasure NextTreasure
    {
      get
      {
        if (NumberOfTreasures > 0)
        {
          return _treasures.Peek();
        }

        throw new InvalidOperationException("There are no extra treasures.");
      }
    }

    public ITreasure PopNextTreasure()
    {
      if (NumberOfTreasures > 0)
      {
        return _treasures.Dequeue();
      }

      throw new InvalidOperationException("There are no extra treasures.");
    }

    private static IList<IPlayerInfo> CopyPlayers(IEnumerable<IPlayerInfo> players)
    {
      return players.Select(CopyPlayer).ToList();
    }

    private static IPlayerInfo CopyPlayer(IPlayerInfo player)
    {
      return new PlayerInfo(player.Color, player.HomePosition, player.CurrentPosition,
        player.CurrentlyAssignedTreasure);
    }
  }
}