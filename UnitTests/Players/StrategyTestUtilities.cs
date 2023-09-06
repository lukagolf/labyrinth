using System.Collections.Generic;
using Common;
using LanguageExt;
using Players;
using Xunit;

namespace UnitTests.Players
{
  internal static class StrategyTestUtilities
  {
    internal static void AssertMoveEquals(Option<Either<IMove, Pass>> result, IMove expectedMove)
    {
      Assert.True(
        result
          .Some(moveOrPass =>
          {
            return moveOrPass.Match(
              _ => false,
              move => move.Equals(expectedMove)
            );
          })
          .None(() => false)
      );
    }

    internal static IPlayerState CreateGameState(ITile spareTile, BoardPosition playerPosition)
    {
      var player = new PublicPlayerInfo(Color.Purple, new BoardPosition(1, 1), playerPosition);
      var players = new List<IPublicPlayerInfo> {player};
      return new PlayerState(players, CreateBoard(), spareTile);
    }

    private static IBoard CreateBoard()
    {
      return new Board(new ITile[,]
      {
        {
          new Tile(false, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },

        {
          new Tile(true, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },

        {
          new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },

        {
          new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },

        {
          new Tile(true, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },

        {
          new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },

        {
          new Tile(true, true, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, false, false, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(false, false, true, true, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
          new Tile(true, true, false, false, new Treasure(Gem.Alexandrite, Gem.Alexandrite)),
        },
      });
    }
  }
}