using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
  public class Tile : ITile
  {
    #region Fields

    private readonly bool _upConnection;
    private readonly bool _leftConnection;
    private readonly bool _downConnection;
    private readonly bool _rightConnection;

    #endregion

    #region Constructor

    public Tile(bool upConnection, bool leftConnection, bool downConnection, bool rightConnection, ITreasure treasure)
    {
      var connections = new List<bool> {upConnection, leftConnection, downConnection, rightConnection};
      int count = connections.Count(conn => conn);
      if (count < 2)
      {
        throw new ArgumentException("Invalid tile, the tile must have at least two connections.");
      }

      _upConnection = upConnection;
      _leftConnection = leftConnection;
      _downConnection = downConnection;
      _rightConnection = rightConnection;

      Treasure = treasure;
    }

    #endregion

    #region Public Interface

    public bool HasUpConnection()
    {
      return _upConnection;
    }

    public bool HasLeftConnection()
    {
      return _leftConnection;
    }

    public bool HasDownConnection()
    {
      return _downConnection;
    }

    public bool HasRightConnection()
    {
      return _rightConnection;
    }

    public ITreasure Treasure { get; }

    public ITile Rotate(Rotation degrees)
    {
      return degrees switch
      {
        Rotation.Zero => this,
        Rotation.Ninety => new Tile(HasLeftConnection(), HasDownConnection(), HasRightConnection(), HasUpConnection(),
          Treasure),
        Rotation.OneHundredEighty => Rotate(Rotation.Ninety).Rotate(Rotation.Ninety),
        Rotation.TwoHundredSeventy => Rotate(Rotation.OneHundredEighty).Rotate(Rotation.Ninety),
        _ => throw new ArgumentException("Unknown rotation degrees")
      };
    }

    #endregion

    public bool Equals(ITile? other)
    {
      if (other == null)
      {
        throw new ArgumentNullException(nameof(other));
      }

      return other.HasUpConnection() == HasUpConnection() &&
             other.HasDownConnection() == HasDownConnection() &&
             other.HasLeftConnection() == HasLeftConnection() &&
             other.HasRightConnection() == HasRightConnection() &&
             other.Treasure.Equals(Treasure);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(_upConnection, _leftConnection, _downConnection, _rightConnection, Treasure);
    }
  }
}