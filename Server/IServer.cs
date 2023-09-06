using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;

namespace Server
{
  /// <summary>
  /// Represents a TCP game server for the Maze game
  /// </summary>
  public interface IServer
  {
    /// <summary>
    /// Waits for remote players to sign up, runs a game to completion if there are enough players, and produces the
    /// result of the game
    /// </summary>
    /// <param name="address">IP address of the TCP server</param>
    /// <param name="port">The port number of the TCP server</param>
    /// <returns>The game result consisting of winning and misbehaved players</returns>
    Task<(IList<string> winningPlayers, IList<string> badPlayers)> RunAsync(IPAddress address, int port);
    
    /// <summary>
    /// Waits for remote players to sign up, runs a game to completion if there are enough players, and produces the
    /// result of the game
    /// </summary>
    /// <param name="address">IP address of the TCP server</param>
    /// <param name="port">The port number of the TCP server</param>
    /// <param name="state">The state to run the game from</param>
    /// <returns>The game result consisting of winning and misbehaved players</returns>
    Task<(IList<string> winningPlayers, IList<string> badPlayers)> RunAsync(IPAddress address, int port,
      IRefereeState state);
  }
}