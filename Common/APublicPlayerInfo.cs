namespace Common
{
  public abstract class APublicPlayerInfo : IPublicPlayerInfo
  {
    private protected APublicPlayerInfo(Color color, BoardPosition homePosition, BoardPosition currentPosition)
    {
      Color = color;
      HomePosition = homePosition;
      CurrentPosition = currentPosition;
    }

    public Color Color { get; }
    public BoardPosition HomePosition { get; }
    public BoardPosition CurrentPosition { get; set; }
  }
}