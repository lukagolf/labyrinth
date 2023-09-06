using System;

namespace Common
{
  public class Treasure : ITreasure
  {
    #region Constructor

    /// <summary>
    /// Constructs an internal representation of the treasure
    /// </summary>
    /// <param name="gem1">First gem</param>
    /// <param name="gem2">Second gem</param>
    public Treasure(Gem gem1, Gem gem2)
    {
      Gem1 = gem1;
      Gem2 = gem2;
    }

    #endregion

    #region Properties

    public Gem Gem1 { get; }
    public Gem Gem2 { get; }

    #endregion

    public bool Equals(ITreasure? other)
    {
      if (other == null) throw new ArgumentNullException(nameof(other));
      return (other.Gem1 == Gem1 && other.Gem2 == Gem2) || (other.Gem1 == Gem2 && other.Gem2 == Gem1);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine((int) Gem1, (int) Gem2);
    }
  }
}