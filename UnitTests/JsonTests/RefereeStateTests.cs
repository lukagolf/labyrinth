using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JsonUtilities;
using LanguageExt;
using Newtonsoft.Json;
using Xunit;

namespace UnitTests.JsonTests
{
  public class RefereeStateTests
  {
    private readonly IRefereeState _state;

    public RefereeStateTests()
    {
      _state = CreateGameState();
    }

    [Fact]
    public void Test()
    {
      RunTest(_state);
    }
    
    private static void RunTest(IRefereeState state)
    {
      var writer = new StringWriter();
      CustomSerializer.Instance.Serialize(writer, state);
      var reader = new StringReader(writer.ToString());
      var jsonReader = new JsonTextReader(reader);
      jsonReader.Read();
      IRefereeState copyState = CustomSerializer.Instance.Deserialize<IRefereeState>(jsonReader)!;
      AssertSameRefereeState(state, copyState);
    }

    private static void AssertSameRefereeState(IRefereeState state1, IRefereeState state2)
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
    
    private static void AssertSamePlayerList(IList<IPlayerInfo> players1, IList<IPlayerInfo> players2)
    {
      Assert.Equal(players1.Count, players2.Count);
      var pairs = players1.Zip(players2);
      foreach (var pair in pairs)
      {
        AssertSamePlayer(pair.Item1, pair.Item2);
      }
    }

    private static void AssertSamePlayer(IPlayerInfo player1, IPlayerInfo player2)
    {
      Assert.Equal(player1.Color, player2.Color);
      Assert.Equal(player1.HomePosition, player2.HomePosition);
      Assert.Equal(player1.CurrentPosition, player2.CurrentPosition);
      Assert.Equal(player1.CurrentlyAssignedTreasure, player2.CurrentlyAssignedTreasure);
    }

    private static void AssertSameLastSlideAction(Option<ISlideAction> slideAction1, Option<ISlideAction> slideAction2)
    {
      Assert.Equal(slideAction1, slideAction2);
    }

    private static void AssertSameTile(ITile tile1, ITile tile2)
    {
      Assert.Equal(tile1, tile2);
    }
    
    private static IRefereeState CreateGameState()
    {
      var purplePlayer = new PlayerInfo(Color.Purple, new BoardPosition(1, 1), new BoardPosition(1, 1),
        new Treasure(Gem.AlexandritePearShape, Gem.Goldstone));

      var orangePlayer = new PlayerInfo(Color.Orange, new BoardPosition(1, 3), new BoardPosition(1, 3),
        new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate));

      var pinkPlayer = new PlayerInfo(Color.Pink, new BoardPosition(1, 5), new BoardPosition(1, 5),
        new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique));

      var players = new List<IPlayerInfo> {purplePlayer, orangePlayer, pinkPlayer};

      ITile spareTile = new Tile(true, false, true, false,
        new Treasure(Gem.AlexandritePearShape, Gem.AlexandritePearShape));

      return new RefereeState(players, CreateBoard(), spareTile);
    }
    
    private static IBoard CreateBoard()
    {
      return new Board(new ITile[,]
      {
        {
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Alexandrite)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.AlmandineGarnet)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Amethyst)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Ametrine)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Ammolite)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Apatite)),
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Aplite)),
        },

        {
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.ApricotSquareRadiant)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Aquamarine)),
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.AustralianMarquise)),
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Aventurine)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Azurite)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Beryl)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.BlackObsidian)),
        },

        {
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BlackOnyx)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.BlackSpinelCushion)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BlueCeylonSapphire)),
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BlueCushion)),
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.BluePearShape)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.BlueSpinelHeart)),
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.BullsEye)),
        },

        {
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Carnelian)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.ChromeDiopside)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.ChrysoberylCushion)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Chrysolite)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.CitrineCheckerboard)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Citrine)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Clinohumite)),
        },

        {
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.ColorChangeOval)),
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Cordierite)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Diamond)),
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Dumortierite)),
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Emerald)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.FancySpinelMarquise)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Garnet)),
        },

        {
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.GoldenDiamondCut)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.Goldstone)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Grandidierite)),
          new Tile(false, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.GrayAgate)),
          new Tile(true, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.GreenAventurine)),
          new Tile(true, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.GreenBerylAntique)),
          new Tile(true, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.GreenBeryl)),
        },

        {
          new Tile(true, true, true, true, new Treasure(Gem.AlexandritePearShape, Gem.GreenPrincessCut)),
          new Tile(true, false, true, false, new Treasure(Gem.AlexandritePearShape, Gem.GrossularGarnet)),
          new Tile(false, true, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Hackmanite)),
          new Tile(false, true, true, false, new Treasure(Gem.AlexandritePearShape, Gem.Heliotrope)),
          new Tile(true, false, false, true, new Treasure(Gem.AlexandritePearShape, Gem.Hematite)),
          new Tile(false, false, true, true, new Treasure(Gem.AlexandritePearShape, Gem.IoliteEmeraldCut)),
          new Tile(true, true, false, false, new Treasure(Gem.AlexandritePearShape, Gem.Jasper)),
        },
      });
    }
  }
}