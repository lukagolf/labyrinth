using Common;

namespace JsonUtilities
{
  public sealed class TakeTurnCall
  {
    public TakeTurnCall(IPlayerState state)
    {
      State = state;
    }

    public IPlayerState State { get; }
  }
}