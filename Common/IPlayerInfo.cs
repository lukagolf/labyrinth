using System.Collections.Generic;
using LanguageExt;

namespace Common
{
  /// <summary>
  /// Represents the player's information in the state that can be observed and mutated only by the Referee
  /// </summary>
  public interface IPlayerInfo : IPublicPlayerInfo
  {
    // Represents the next treasure this player needs to chase
    public ITreasure CurrentlyAssignedTreasure { get; set; }

    public int NumberOfCapturedTreasures { get; set; }

    // The round index of when the player has captured all of its assigned treasures, if they did
    // It is one of:
    // - Some<int>, if the player has captured all of its assigned treasures
    // - None<int>, if the player did not capture all of its assigned treasures
    public Option<int> CapturedAllAssignedTreasuresRoundIdx { get; set; }
  }
}