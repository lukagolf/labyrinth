using Common;

namespace Observer
{
  /// <summary>
  /// Represents a function object that can draw the given game state within the given dimension constraints
  /// </summary>
  /// <typeparam name="T">The image type parameter</typeparam>
  public interface IStateDrawer<T>
  {
    /// <summary>
    /// Draws the given state as an image
    /// </summary>
    /// <param name="state">The state to draw</param>
    /// <param name="maxPixelWidth">The maximum width of the image</param>
    /// <param name="maxPixelHeight">The maximum height of the image</param>
    /// <returns>The image</returns>
    public T Draw(IRefereeState state, int maxPixelWidth, int maxPixelHeight);
  }
}