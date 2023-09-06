using System;

namespace Common
{
  /// <summary>
  /// Represents operations that a tile must support
  /// </summary>
  public interface ITile : IEquatable<ITile>
  {
    /// <summary>
    /// Does this tile have an up connection?
    /// </summary>
    /// <returns>A bool representing if the tile has an up connection</returns>
    public bool HasUpConnection();

    /// <summary>
    /// Does this tile have a left connection?
    /// </summary>
    /// <returns>A bool representing if the tile has a left connection</returns>
    public bool HasLeftConnection();

    /// <summary>
    /// Does this tile have a down connection?
    /// </summary>
    /// <returns>A bool representing if the tile has a down connection</returns>
    public bool HasDownConnection();

    /// <summary>
    /// Does this tile have a right connection?
    /// </summary>
    /// <returns>A bool representing if the tile has a right connection</returns>
    public bool HasRightConnection();

    public ITreasure Treasure { get; }

    /// <summary>
    /// Produces a new rotated version of this tile by rotating it clockwise 
    /// </summary>
    /// <param name="degrees">Number of degrees to rotate this tile clockwise</param>
    /// <returns>A new rotated version of this tile</returns>
    public ITile Rotate(Rotation degrees);
  }
}