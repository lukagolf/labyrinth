using Common;

namespace UnitTests.Common
{
  internal static class BoardUtils
  {
    public static IBoard CreateBoard()
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