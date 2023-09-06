using System;

namespace Common
{
  /// <summary>
  /// Represents the slide action that is performed by inserting the rotated spare tile in the specified row or column
  /// index.
  /// </summary>
  public interface ISlideAction : IEquatable<ISlideAction>
  {
    /// <summary>
    /// Represents the type of sliding : Sliding row left/right or sliding a column down/up.
    /// </summary>
    public SlideType Type { get; }
    
    /// <summary>
    /// Represents the row or column index at which slide type is performed. When sliding a row, index is interpreted
    /// as row index, when sliding a column, index is interpreted as column index.
    /// </summary>
    public int Index { get; }
  }
}