using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JsonUtilities;
using Newtonsoft.Json;
using Players;
using Referee;

namespace GameIntegrationTests
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
      
      IList<IPlayer> players = ReadPlayers(jsonReader, serializer);
      IRefereeState state = ReadState(jsonReader, serializer);

      var winners = RunGame(players, state);

      serializer.Serialize(jsonWriter, winners);
    }

    private static IList<IPlayer> ReadPlayers(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<IList<IPlayer>>(reader)!;
    }

    private static IRefereeState ReadState(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<IRefereeState>(reader)!;
    }

    private static IEnumerable<string> RunGame(IList<IPlayer> players, IRefereeState state)
    {
      IReferee referee = new Referee.Referee(new RuleBook(), 10000);
      var result = referee.RunGame(state, players);
      return result.winningPlayers.Select(winner => winner.Name).OrderBy(n => n);
    }
    
    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new PlayerJsonConverter());
      serializer.Converters.Add(new RefereeStateJsonConverter(false));
      return serializer;
    }
  }
}