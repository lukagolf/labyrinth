using System;
using System.IO;
using Common;
using JsonUtilities;
using LanguageExt;
using Newtonsoft.Json;
using Players;

namespace StrategyIntegrationTests
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

      StrategyTypeJson strategyTypeJson = ReadStrategyType(jsonReader, serializer);
      IPlayerState state = ReadState(jsonReader, serializer);
      BoardPosition goal = ReadGoal(jsonReader, serializer);
      
      IPlayerStrategy strategy = CreateStrategy(strategyTypeJson);
      IRule rule = new RuleBook();
      Either<IMove, Pass> maybeMove = strategy.ChooseMove(state, rule, goal).ToMaybeMove();

      serializer.Serialize(jsonWriter, maybeMove);
    }

    private static StrategyTypeJson ReadStrategyType(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<StrategyTypeJson>(reader);
    }

    private static IPlayerState ReadState(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<IPlayerState>(reader)!;
    }

    private static BoardPosition ReadGoal(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<BoardPosition>(reader)!;
    }

    private static IPlayerStrategy CreateStrategy(StrategyTypeJson strategyType)
    {
      return strategyType switch
      {
        StrategyTypeJson.Riemann => new RiemannStrategy(),
        StrategyTypeJson.Euclid => new EuclidStrategy(),
        _ => throw new ArgumentOutOfRangeException(nameof(strategyType)),
      };
    }
    
    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new PlayerStateJsonConverter(false));
      serializer.Converters.Add(new BoardPositionJsonConverter());
      serializer.Converters.Add(new MoveJsonConverter());
      return serializer;
    }
  }
}