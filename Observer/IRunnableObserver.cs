using Referee;

namespace Observer
{
  /// <summary>
  /// Represents a runnable observer that needs to be run explicitly to display its observations
  /// </summary>
  public interface IRunnableObserver : IObserver
  {
    /// <summary>
    /// Runs this observer
    /// </summary>
    public void Run();
  }
}