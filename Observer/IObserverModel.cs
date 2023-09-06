using Common;
using LanguageExt;

namespace Observer
{
  /// <summary>
  /// Represent a data structure the observers can use to store the game states 
  /// </summary>
  public interface IObserverModel
  {
    /// <summary>
    /// Gets the current state of the game.
    /// It is one of:
    /// None: If there is no game state stored in the model
    /// Some(IState): If there is a game state stored in the model
    /// </summary>
    public Option<IRefereeState> CurrentState { get; }
    
    /// <summary>
    /// Pushes the given game state to the end of the game state collection if IsGameOver = false
    /// </summary>
    /// <param name="state">The state to add</param>
    public void PushState(IRefereeState state);
    
    /// <summary>
    /// Sets the current state to the next state in the collection if there is a next state
    /// </summary>
    public void Next();
    
    /// <summary>
    /// Is there a game state after the current state?
    /// </summary>
    public bool HasNext();
    
    /// <summary>
    /// Is the game over?
    /// </summary>
    public bool IsGameOver { get; }
    
    /// <summary>
    /// Locks the model so that it won't add new states to the collection
    /// </summary>
    public void SetGameOver();
  }
}