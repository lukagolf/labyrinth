using System;

namespace Common
{
  /// <summary>
  /// Represents an atomic move that a player can perform during the game.
  /// Atomic move consists of three things:
  /// 1. The rotation of the spare tile in clockwise direction in degrees
  /// 2. The sliding of the row or column by inserting the rotated spare tile.
  /// 3. Moving the player to the destination board position.
  /// </summary>
  public interface IMove : IEquatable<IMove>
  {
    /// <summary>
    /// The rotation of the spare tile in clockwise direction in degrees
    /// </summary>
    public Rotation Rotation { get; }
    
    /// <summary>
    /// The sliding of the row or column by inserting the rotated spare tile.
    /// </summary>
    public ISlideAction Slide { get; }
   
    /// <summary>
    /// Moving the player to the destination board position.
    /// </summary>
    public BoardPosition Destination { get; }
  }
}