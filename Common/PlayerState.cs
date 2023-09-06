using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace Common
{
  public sealed class PlayerState : AState<IPublicPlayerInfo>, IPlayerState
  {
    public PlayerState(IEnumerable<IImmutablePublicPlayerInfo> players, IImmutableBoard board, ITile spareTile,
      bool distinctHomes = true) : this(CopyPlayers(players), board, spareTile, Option<ISlideAction>.None,
      distinctHomes)
    {
    }

    public PlayerState(IEnumerable<IImmutablePublicPlayerInfo> players, IImmutableBoard board, ITile spareTile,
      Option<ISlideAction> lastSlideAction, bool distinctHomes = true) : base(CopyPlayers(players), board, spareTile,
      lastSlideAction, distinctHomes)
    {
    }


    IPlayerState IPlayerState.RotateSpareTile(Rotation rotation)
    {
      PlayerState copy = Copy();
      copy.RotateSpareTile(rotation);
      return copy;
    }

    IPlayerState IPlayerState.PerformSlide(ISlideAction slideAction)
    {
      PlayerState copy = Copy();
      copy.PerformSlide(slideAction);
      return copy;
    }

    private PlayerState Copy() => new(AllPlayers, Board, SpareTile, LastSlideAction, false);

    private static IList<IPublicPlayerInfo> CopyPlayers(IEnumerable<IImmutablePublicPlayerInfo> players)
    {
      return players.Select(CopyPlayer).ToList();
    }

    private static IPublicPlayerInfo CopyPlayer(IImmutablePublicPlayerInfo player)
    {
      return new PublicPlayerInfo(player.Color, player.HomePosition, player.CurrentPosition);
    }
  }
}