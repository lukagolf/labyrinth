using System;
using System.Collections.Generic;
using System.IO;
using Common;
using JsonUtilities;
using Newtonsoft.Json;

namespace BoardIntegrationTests
{
  public static class Program
  {
    public static void Main()
    {
      RunTest(Console.In, Console.Out);
    }

    private static void RunTest(TextReader reader, TextWriter writer)
    {
      var jsonReader = new JsonTextReader(reader) {SupportMultipleContent = true};
      var jsonWriter = new JsonTextWriter(writer);
      JsonSerializer serializer = CreateSerializer();

      (IBoard board, BoardPosition source) = ParseTestInputs(jsonReader, serializer);
      IEnumerable<BoardPosition> reachablePoints = board.GetAllReachablePositions(source);

      serializer.Serialize(jsonWriter, reachablePoints);
      writer.WriteLine();
    }

    private static (IBoard board, BoardPosition source) ParseTestInputs(JsonReader jsonReader,
      JsonSerializer serializer)
    {
      jsonReader.Read();
      IBoard board = serializer.Deserialize<IBoard>(jsonReader)!;

      jsonReader.Read();
      BoardPosition source = serializer.Deserialize<BoardPosition>(jsonReader);

      return (board, source);
    }

    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new BoardJsonConverter());
      serializer.Converters.Add(new BoardPositionJsonConverter());
      return serializer;
    }
  }
}