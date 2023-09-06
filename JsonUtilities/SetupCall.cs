using Common;
using LanguageExt;

namespace JsonUtilities
{
  public sealed class SetupCall
  {
    public SetupCall(Option<IPlayerState> state, BoardPosition goal)
    {
      State = state;
      Goal = goal;
    }

    public Option<IPlayerState> State { get; }
    public BoardPosition Goal { get; }
  }
}