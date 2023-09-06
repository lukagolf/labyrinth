using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JsonUtilities;
using Newtonsoft.Json;
using Players;
using Referee;

namespace BadIntegrationTests
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

      var result = RunGame(players, state);
      var output = new List<IList<string>> {result.winners, result.badPlayers};

      serializer.Serialize(jsonWriter, output);
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

    private static (IList<string> winners, IList<string> badPlayers) RunGame(IList<IPlayer> players,
      IRefereeState state)
    {
      IReferee referee = new Referee.Referee(new RuleBook(), 4000);
      var result = referee.RunGame(state, players);
      return (
        result.winningPlayers.Select(w => w.Name).OrderBy(n => n).ToList(),
        result.misbehavedPlayers.Select(b => b.Name).OrderBy(n => n).ToList()
      );
    }

    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new MixedPlayerJsonConverter());
      serializer.Converters.Add(new RefereeStateJsonConverter(false));
      return serializer;
    }
  }
}