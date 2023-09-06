using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Players;
using Remote;

namespace Client
{
  /// <summary>
  /// Represents a TCP client that can connect multiple remote players to a game server
  /// </summary>
  public interface IMultiClient
  {
    /// <summary>
    /// Connects the given list of players to the remote server running on the given IP address and port number 
    /// </summary>
    /// <param name="server">IP address of the server</param>
    /// <param name="port">Port number of the server</param>
    /// <param name="players">The players to connect to</param>
    /// <returns>Player-dispatcher result of each player</returns>
    public Task<IList<Result>> RunAsync(IPAddress server, int port, IList<IPlayer> players);
  }
}