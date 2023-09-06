using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Players;
using Remote;

namespace Client
{
  public sealed class MultiClient : IMultiClient
  {
    private readonly IClient _client;
    private readonly int _clientLaunchDelayMilliseconds;

    /// <param name="client">Client implementation to use to connect a single player</param>
    /// <param name="clientLaunchDelayMilliseconds">Number of milliseconds to wait between launching clients</param>
    public MultiClient(IClient client, int clientLaunchDelayMilliseconds = 3000)
    {
      _client = client;
      _clientLaunchDelayMilliseconds = clientLaunchDelayMilliseconds;
    }

    public async Task<IList<Result>> RunAsync(IPAddress server, int port, IList<IPlayer> players)
    {
      async Task<Result> WaitAndRun(IPlayer player, int idx)
      {
        await Task.Delay(_clientLaunchDelayMilliseconds * idx);
        return await _client.RunAsync(server, port, player);
      }

      IEnumerable<Task<Result>> tasks = players.Select(WaitAndRun);

      return await Task.WhenAll(tasks);
    }
  }
}