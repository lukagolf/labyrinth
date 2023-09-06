using System.Net;
using System.Threading.Tasks;
using Players;
using Remote;

namespace Client
{
  /// <summary>
  /// Represents a TCP client that can connect a single remote player to a game server
  /// </summary>
  public interface IClient
  {
    /// <summary>
    /// Connects the given player to the remote server running on the given IP address and port number 
    /// </summary>
    /// <param name="server">IP address of the server</param>
    /// <param name="port">Port number of the server</param>
    /// <param name="player">The player to connect to</param>
    /// <returns>Player-dispatcher result</returns>
    public Task<Result> RunAsync(IPAddress server, int port, IPlayer player);
  }
}