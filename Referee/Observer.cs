using System.Threading.Tasks;
using Common;

namespace Referee
{
  /// <summary>
  /// Represents an observer that the Referee sends updates about the current game state 
  /// </summary>
  public interface IObserver
  {
    /// <summary>
    /// Notifies this observer about the current state of the game
    /// </summary>
    /// <param name="state">The current state of the game</param>
    /// <returns>An acknowledgement indicating that the observer got the game state update</returns>
    public Acknowledge PushState(IRefereeState state);

    /// <summary>
    /// Notifies this observer that the game ended
    /// </summary>
    /// <returns>An acknowledgement indicating that the observer got the game over notification</returns>
    public Acknowledge NotifyGameOver();
  }
}