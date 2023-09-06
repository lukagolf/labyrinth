using System;

namespace Common
{
  public class Move : IMove
  {
    public Move(ISlideAction slide, Rotation rotation, BoardPosition destination)
    {
      Slide = slide;
      Rotation = rotation;
      Destination = destination;
    }

    public ISlideAction Slide { get; }
    public Rotation Rotation { get; }
    public BoardPosition Destination { get; }

    public bool Equals(IMove? other)
    {
      if (other is null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      return Slide.Equals(other.Slide) && Rotation == other.Rotation && Destination.Equals(other.Destination);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Slide, (int) Rotation, Destination);
    }
  }
}