using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Client;
using System.Threading.Tasks;
using Common;
using JsonUtilities;
using Newtonsoft.Json;
using Players;
using Remote;

namespace RunnableClient
{
  public static class Program
  {
    private const int ClientLaunchDelayMilliseconds = 3000;
    private const int ReconnectInterval = 100;

    public static async Task Main(string[] args)
    {
      int port = Convert.ToInt32(args[0]);
      IPAddress serverIp = IPAddress.Parse("127.0.0.1");
      if (args.Length > 1)
      {
        serverIp = IPAddress.Parse(args[1]);
      }

      var players = await ReadPlayers(Console.In);
      var result = await RunClient(players, serverIp, port);
      PrintResult(Console.Out, players, result);
    }

    private static async Task<IList<IPlayer>> ReadPlayers(TextReader reader)
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new MixedPlayerJsonConverter());
      var jsonReader = new JsonTextReader(reader);
      await jsonReader.ReadAsync();
      return serializer.Deserialize<IList<IPlayer>>(jsonReader)!;
    }

    private static async Task<IList<Result>> RunClient(IList<IPlayer> players, IPAddress serverIp, int port)
    {
      IPlayerDispatcher playerDispatcher = new PlayerDispatcher();
      IClient client = new Client.Client(playerDispatcher, ReconnectInterval);
      IMultiClient multiClient = new MultiClient(client, clientLaunchDelayMilliseconds: ClientLaunchDelayMilliseconds);
      return await multiClient.RunAsync(serverIp, port, players);
    }

    private static void PrintResult(TextWriter writer, IList<IPlayer> players, IList<Result> results)
    {
      IEnumerable<string> lines = players.Zip(results)
        .Select(pair => $"Player: {pair.Item1.Name}, Result: {ResultToString(pair.Item2)}");
      lines.ForEach(writer.WriteLine);
    }

    private static string ResultToString(Result result)
    {
      return result.Value.Match(
        _ => "Read Error",
        _ => "Write Error",
        _ => "Player Error",
        won => won ? "Won" : "Lost"
      );
    }
  }
}