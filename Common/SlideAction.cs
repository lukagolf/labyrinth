using System;

namespace Common
{
  public class SlideAction : ISlideAction
  {
    public SlideAction(SlideType type, int index)
    {
      Type = type;
      Index = index;
    }

    public SlideType Type { get; }
    public int Index { get; }

    public bool Equals(ISlideAction? other)
    {
      if (other is null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      return Type == other.Type && Index == other.Index;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine((int) Type, Index);
    }
  }
}