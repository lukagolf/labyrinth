namespace Observer
{
  /// <summary>
  /// Represents the high level events that a graphical observer view can trigger
  /// </summary>
  internal interface IViewEvents
  {
    /// <summary>
    /// Handles the next state event. This can be a key or a button press depending on the view implementation
    /// </summary>
    public void OnNext();
    
    /// <summary>
    /// Handles the save state event. This can be a key or a button press depending on the view implementation
    /// </summary>
    /// <param name="filePath">The file path where the state will be exported</param>
    public void OnSave(string filePath);
  }
}