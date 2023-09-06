using System;
using System.Collections.Generic;
using System.IO;
using Common;
using JsonUtilities;
using Newtonsoft.Json;

namespace StateIntegrationTests
{
  public static class Program
  {
    public static void Main()
    {
      RunTest(Console.In, Console.Out);
      Console.WriteLine();
    }

    private static void RunTest(TextReader reader, TextWriter writer)
    {
      var jsonReader = new JsonTextReader(reader) {SupportMultipleContent = true};
      var jsonWriter = new JsonTextWriter(writer);
      JsonSerializer serializer = CreateSerializer();

      IPlayerState state = ReadState(jsonReader, serializer);
      ISlideAction slideAction = ReadSlideAction(jsonReader, serializer);
      Rotation rotation = ReadRotation(jsonReader, serializer);

      var reachablePoints = FindReachablePoints(state, slideAction, rotation);

      serializer.Serialize(jsonWriter, reachablePoints);
    }

    private static IEnumerable<BoardPosition> FindReachablePoints(IPlayerState state, ISlideAction slideAction,
      Rotation rotation)
    {
      return state.RotateSpareTile(rotation).PerformSlide(slideAction).AllReachablePositionsByActivePlayer;
    }

    private static IPlayerState ReadState(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<IPlayerState>(reader)!;
    }

    private static ISlideAction ReadSlideAction(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      int index = serializer.Deserialize<int>(reader);
      reader.Read();
      SlideType slideType = serializer.Deserialize<SlideType>(reader);
      return new SlideAction(slideType, index);
    }

    private static Rotation ReadRotation(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<Rotation>(reader);
    }

    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new PlayerStateJsonConverter(false));
      serializer.Converters.Add(new SlideTypeJsonConverter());
      serializer.Converters.Add(new RotationJsonConverter());
      serializer.Converters.Add(new BoardPositionJsonConverter());
      return serializer;
    }
  }
}