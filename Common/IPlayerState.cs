namespace Common
{
  /// <summary>
  /// Represents the players' game state. Operations on this interface only allow access to public information
  /// that should be accessible to all players. It extends IState with additional functionalities that players can use
  /// to experiment with different moves
  /// </summary>
  public interface IPlayerState : IState
  {
    /// <summary>
    /// Rotates the spare tile in clock-wise direction by the given number of degrees
    /// </summary>
    /// <param name="rotation">The number of degrees to rotate by</param>
    /// <returns>A new state where the spare tile has been rotated</returns>
    public IPlayerState RotateSpareTile(Rotation rotation);

    /// <summary>
    /// Performs the given slide action on this state
    /// </summary>
    /// <param name="slideAction">The slide action to perform</param>
    /// <returns>A new state where the given slide action has been performed</returns>
    public IPlayerState PerformSlide(ISlideAction slideAction);
  }
}