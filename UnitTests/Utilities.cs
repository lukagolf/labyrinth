using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;
using Xunit;

namespace UnitTests
{
  public static class Utilities
  {
    public static void AssertSamePlayerState(IPlayerState state1, IPlayerState state2)
    {
      AssertSameBoard(state1.Board, state2.Board);
      AssertSamePlayerList(state1.AllPlayers.ToList(), state2.AllPlayers.ToList());
      AssertSameTile(state1.SpareTile, state2.SpareTile);
      AssertSameLastSlideAction(state1.LastSlideAction, state2.LastSlideAction);
    }

    private static void AssertSameBoard(IImmutableBoard board1, IImmutableBoard board2)
    {
      var pairs = board1.Positions.Zip(board2.Positions);
      foreach (var pair in pairs)
      {
        ITile tile1 = board1.GetTileAt(pair.Item1);
        ITile tile2 = board1.GetTileAt(pair.Item2);
        AssertSameTile(tile1, tile2);
      }
    }
    
    private static void AssertSamePlayerList(IList<IImmutablePublicPlayerInfo> players1, IList<IImmutablePublicPlayerInfo> players2)
    {
      Assert.Equal(players1.Count, players2.Count);
      var pairs = players1.Zip(players2);
      foreach (var pair in pairs)
      {
        AssertSamePlayer(pair.Item1, pair.Item2);
      }
    }

    private static void AssertSamePlayer(IImmutablePublicPlayerInfo player1, IImmutablePublicPlayerInfo player2)
    {
      Assert.Equal(player1.Color, player2.Color);
      Assert.Equal(player1.HomePosition, player2.HomePosition);
      Assert.Equal(player1.CurrentPosition, player2.CurrentPosition);
    }

    private static void AssertSameLastSlideAction(Option<ISlideAction> slideAction1, Option<ISlideAction> slideAction2)
    {
      Assert.Equal(slideAction1, slideAction2);
    }

    private static void AssertSameTile(ITile tile1, ITile tile2)
    {
      Assert.Equal(tile1, tile2);
    }
  }
}