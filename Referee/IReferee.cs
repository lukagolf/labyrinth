using System.Collections.Generic;
using Common;
using Players;

namespace Referee
{
  /// <summary>
  /// Represents a referee that administers the maze game
  /// </summary>
  public interface IReferee
  {
    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IList<IPlayer> players);

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IList<IPlayer> players,
      IList<IObserver> observers);

    /// <summary>
    /// Runs the maze game from the given state to completion with the given list of players 
    /// </summary>
    /// <param name="state">The initial state to start from</param>
    /// <param name="players">Players who want to participate</param>
    /// <returns>The list of players who won, and the list of players who misbehaved</returns>
    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IRefereeState state,
      IList<IPlayer> players);

    /// <summary>
    /// Runs the maze game from the given state to completion with the given list of players 
    /// </summary>
    /// <param name="state">The initial state to start from</param>
    /// <param name="players">Players who want to participate</param>
    /// <param name="observers">The list of observers that will get notified with game state updates</param>
    /// <returns>The list of players who won, and the list of players who misbehaved</returns>
    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IRefereeState state,
      IList<IPlayer> players, IList<IObserver> observers);
  }
}