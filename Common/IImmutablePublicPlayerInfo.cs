namespace Common
{
  public interface IImmutablePublicPlayerInfo
  {
    public Color Color { get; }
    public BoardPosition HomePosition { get; }
    public BoardPosition CurrentPosition { get; }
  }
}