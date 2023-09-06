namespace Common
{
  public sealed class PublicPlayerInfo : APublicPlayerInfo
  {
    public PublicPlayerInfo(Color color, BoardPosition homePosition, BoardPosition currentPosition) : base(color,
      homePosition, currentPosition)
    {
    }
  }
}