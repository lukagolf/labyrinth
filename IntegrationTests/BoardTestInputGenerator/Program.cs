using System;
using System.Collections.Generic;
using Common;
using JsonUtilities;
using Newtonsoft.Json;

namespace BoardTestInputGenerator
{
  public static class Program
  {
    private static readonly IList<string> Connectors = new List<string>
    {
      "│", "─", "┐", "└", "┌", "┘", "┬", "├", "┴", "┤", "┼"
    };
    
    private static readonly IList<string> Gems = new List<string>
    {
      "ammolite", "aquamarine", "sunstone", "ruby", "prehnite", "magnesite", "grandidierite"
    };

    private static readonly Random Random = new();

    public static void Main()
    {
      var boardJson = GenerateBoard();
      var point = GeneratePoint();
      Console.WriteLine(JsonConvert.SerializeObject(boardJson));
      Console.WriteLine(JsonConvert.SerializeObject(point, Formatting.Indented));
    }

    private static BoardJson GenerateBoard()
    {
      return new BoardJson(GenerateConnectors(), GenerateTreasures());
    }

    private static CoordinateJson GeneratePoint()
    {
      return new CoordinateJson(Random.Next(8), Random.Next(8));
    }

    private static string[,] GenerateConnectors()
    {
      var connectors = new string[7, 7];
      connectors.Fill((_, _) => GenerateSingleConnector());
      return connectors;
    }

    private static IList<string>[,] GenerateTreasures()
    {
      var treasures = new IList<string>[7, 7];
      treasures.Fill((_, _) => GenerateSingleTreasure());
      return treasures;
    }

    private static string GenerateSingleConnector()
    {
      return Connectors[Random.Next(Connectors.Count)];
    }

    private static IList<string> GenerateSingleTreasure()
    {
      return new List<string> {GenerateGem(), GenerateGem()};
    }

    private static string GenerateGem()
    {
      return Gems[Random.Next(Gems.Count)];
    }
  }
}