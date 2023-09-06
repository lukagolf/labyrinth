namespace Common
{
  /// <summary>
  /// Represents the player's information that is visible to all external players and observers of the game.
  /// </summary>
  public interface IPublicPlayerInfo : IImmutablePublicPlayerInfo
  {
    public new BoardPosition CurrentPosition { get; set; }
  }
}