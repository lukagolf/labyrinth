using System.IO;
using Common;
using Eto.Drawing;
using Eto.Forms;
using JsonUtilities;

namespace Observer
{
  /// <summary>
  /// Provides a IRunnableObserver implementation that have the following pieces of functionality:
  /// - Render the current state of the game as an image inside a gui canvas
  /// - Show the next state of the game upon a button click
  /// - Save the current state of the game to a file as json upon a button click
  /// </summary>
  public sealed class GuiObserver : IRunnableObserver, IViewEvents
  { private readonly Application _app;
    private readonly ObserverForm _form;
    private readonly IObserverModel _model;
    private readonly IStateDrawer<Image> _stateDrawer;
    private readonly int _width;
    private readonly int _height;

    public GuiObserver(IObserverModel model, IStateDrawer<Image> stateDrawer, int width, int height)
    {
      _app = new Application(Eto.Platforms.Gtk);
      _form = new ObserverForm(this, width, height);
      _model = model;
      _stateDrawer = stateDrawer;
      _width = width;
      _height = height;
    }

    public Acknowledge PushState(IRefereeState state)
    {
      _model.PushState(state);
      return Acknowledge.Value;
    }

    public Acknowledge NotifyGameOver()
    {
      _model.SetGameOver();
      return Acknowledge.Value;
    }

    public void Run()
    {
      _app.Run(_form);
    }

    public void OnNext()
    {
      _model.Next();
      _model.CurrentState.IfSome(state =>
      {
        Image stateImage = _stateDrawer.Draw(state, _width, _height);
        _form.Update(stateImage, !_model.HasNext() && _model.IsGameOver);
      });
    }

    public void OnSave(string filePath)
    {
      var maybeState = _model.CurrentState;
      maybeState.IfSome(state =>
      {
        using var writer = new StreamWriter(File.Create(filePath));
        CustomSerializer.Instance.Serialize(writer, state);
      });
    }
  }
}