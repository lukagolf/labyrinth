using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Server;
using System.Threading.Tasks;
using Common;
using JsonUtilities;
using Newtonsoft.Json;
using Players;
using Referee;


namespace RunnableServer
{
  public static class Program
  {
    private const int RefereeTimeoutMilliSeconds = 4000;
    private const int SignUpTimeoutSeconds = 20;
    private const int NameTimeoutSeconds = 2;
    private const int MaxSignUpPeriods = 2;
    private const int MinPlayers = 2;
    private const int MaxPlayers = 6;

    public static async Task Main(string[] args)
    {
      int port = Convert.ToInt32(args[0]);
      var serializer = CreateSerializer();
      IRefereeState state = await ReadState(Console.In, serializer);
      var result = await RunServer(state, port);
      await PrintResult(Console.Out, result, serializer);
    }

    private static async Task<IRefereeState> ReadState(TextReader reader,
      JsonSerializer serializer)
    {
      var jsonReader = new JsonTextReader(reader);
      await jsonReader.ReadAsync();
      return serializer.Deserialize<IRefereeState>(jsonReader)!;
    }

    private static async Task<(IList<string> winningPlayers, IList<string> badPlayers)> RunServer(
      IRefereeState state, int port)
    {
      IReferee referee = new Referee.Referee(new RuleBook(), RefereeTimeoutMilliSeconds);
      IServer server = new Server.Server(referee, signUpTimeout: SignUpTimeoutSeconds, nameTimeout: NameTimeoutSeconds,
        maxSignUpPeriods: MaxSignUpPeriods, minPlayers: MinPlayers, maxPlayers: MaxPlayers);
      var result = await server.RunAsync(IPAddress.Any, port, state);

      return (
        result.winningPlayers.OrderBy(n => n).ToList(),
        result.badPlayers.OrderBy(n => n).ToList()
      );
    }

    private static async Task PrintResult(TextWriter writer,
      (IList<string> winningPlayers, IList<string> badPlayers) result, JsonSerializer serializer)
    {
      var output = new List<IList<string>> {result.winningPlayers, result.badPlayers};
      serializer.Serialize(writer, output);
      await writer.WriteLineAsync();
    }

    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new RefereeStateJsonConverter());
      return serializer;
    }
  }
}