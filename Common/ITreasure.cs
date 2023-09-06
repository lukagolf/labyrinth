using System;

namespace Common
{
  /// <summary>
  /// Represents a treasure inside a tile
  /// </summary>
  public interface ITreasure : IEquatable<ITreasure>
  {
    public Gem Gem1 { get; }
    public Gem Gem2 { get; }
  }
}