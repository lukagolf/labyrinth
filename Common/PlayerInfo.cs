using LanguageExt;

namespace Common
{
  public sealed class PlayerInfo : APublicPlayerInfo, IPlayerInfo
  {
    /// <summary>
    /// Constructs the internal representation of the player with its identifiers
    /// </summary>
    /// <param name="color">The color of the player</param>
    /// <param name="homePosition">The board position of where the player's home is located</param>
    /// <param name="currentPosition">The board position of where the player is currently located</param>
    /// <param name="assignedTreasure">The player's assigned treasure</param>
    public PlayerInfo(Color color, BoardPosition homePosition, BoardPosition currentPosition,
      ITreasure assignedTreasure) : this(color, homePosition, currentPosition, assignedTreasure, 0, Option<int>.None)
    {
    }

    /// <summary>
    /// Constructs the internal representation of the player with its identifiers
    /// </summary>
    /// <param name="color">The color of the player</param>
    /// <param name="homePosition">The board position of where the player's home is located</param>
    /// <param name="currentPosition">The board position of where the player is currently located</param>
    /// <param name="assignedTreasure">The player's assigned treasure</param>
    /// <param name="numberOfCapturedTreasures">The number of treasures this player has captured so far</param>
    /// <param name="capturedAllAssignedTreasuresRoundIdx">
    /// The round index of when the player has captured all of its assigned treasures, if they did
    /// </param>
    private PlayerInfo(Color color, BoardPosition homePosition, BoardPosition currentPosition,
      ITreasure assignedTreasure, int numberOfCapturedTreasures, Option<int> capturedAllAssignedTreasuresRoundIdx) :
      base(color, homePosition, currentPosition)
    {
      CurrentlyAssignedTreasure = assignedTreasure;
      NumberOfCapturedTreasures = numberOfCapturedTreasures;
      CapturedAllAssignedTreasuresRoundIdx = capturedAllAssignedTreasuresRoundIdx;
    }

    public ITreasure CurrentlyAssignedTreasure { get; set; }
    public int NumberOfCapturedTreasures { get; set; }
    public Option<int> CapturedAllAssignedTreasuresRoundIdx { get; set; }
  }
}