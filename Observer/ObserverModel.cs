using System.Collections.Concurrent;
using Common;
using LanguageExt;

namespace Observer
{
  /// <summary>
  /// Provides a thread-safe implementation for IObserverModel using a ConcurrentQueue and blocking locks
  /// </summary>
  public sealed class ObserverModel : IObserverModel
  {
    /// <summary>
    /// Stores the next states in a queue that will come after the _currentState
    /// </summary>
    private readonly ConcurrentQueue<IRefereeState> _states;
    
    /// <summary>
    /// Contains the current game state
    /// </summary>
    private readonly ProtectedValue<Option<IRefereeState>> _currentState;
    
    /// <summary>
    /// Contains the information of whether game is over
    /// </summary>
    private readonly ProtectedValue<bool> _isGameOver;

    public ObserverModel()
    {
      _states = new ConcurrentQueue<IRefereeState>();
      _currentState = new ProtectedValue<Option<IRefereeState>>(Option<IRefereeState>.None);
      _isGameOver = new ProtectedValue<bool>(false);
    }

    public void PushState(IRefereeState state)
    {
      if (!IsGameOver)
      {
        _states.Enqueue(state);
      }
    }

    public Option<IRefereeState> CurrentState => _currentState.Value;

    public void Next()
    {
      if (!HasNext())
      {
        return;
      }

      _states.TryDequeue(out var state);
      _currentState.Value = Option<IRefereeState>.Some(state!);
    }

    public bool HasNext() => !_states.IsEmpty;

    public bool IsGameOver => _isGameOver.Value;

    public void SetGameOver()
    {
      _isGameOver.Value = true;
    }
    
    private sealed class ProtectedValue<T>
    {
      private readonly object _mutex;
      private T _value;

      public ProtectedValue(T value)
      {
        _mutex = new object();
        _value = value;
      }

      public T Value
      {
        get
        {
          lock (_mutex)
          {
            return _value;
          }
        }
        set
        {
          lock (_mutex)
          {
            _value = value;
          }
        }
      }
    }
  }
}