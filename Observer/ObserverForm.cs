using Eto.Drawing;
using Eto.Forms;

namespace Observer
{
  /// <summary>
  /// Represents a form that can display the given game state inside a gui canvas
  /// </summary>
  internal sealed class ObserverForm : Form
  {
    private const int ButtonHeight = 50;
    private const float TextFontSize = 16f;
    
    private readonly IViewEvents _events;
    private readonly ImageView _imageView;
    private readonly Label _gameStatusLabel;

    /// <summary>
    /// Constructs a new ObserverForm that will display the given state image
    /// </summary>
    /// <param name="events">Event handlers to call when the events are triggered</param>
    /// <param name="width">Width of the form in pixels</param>
    /// <param name="height">Height of the form in pixels</param>
    public ObserverForm(IViewEvents events, int width, int height)
    {
      _events = events;
      _imageView = new ImageView()
      {
        Height = height - ButtonHeight
      };
      _gameStatusLabel = CreateGameStatusLabel();

      Size = new Size(width, height);
      Content = CreateContent();
      Resizable = false;
      Maximizable = false;
    }

    public void Update(Image stateImage, bool gameOver)
    {
      ShowStateImage(stateImage);
      UpdateGameOver(gameOver);
    }

    private void ShowStateImage(Image stateImage)
    {
      _imageView.Image = stateImage;
      _imageView.Size = new Size(stateImage.Width, stateImage.Height);
      Size = new Size(stateImage.Width, stateImage.Height + ButtonHeight);
    }

    private void UpdateGameOver(bool gameOver)
    {
      string gameStatus = "Status: " + (gameOver ? "Game Over" : "Game In Progress");
      _gameStatusLabel.Text = gameStatus;
    }

    private Panel CreateContent()
    {
      return new StackLayout()
      {
        Orientation = Orientation.Vertical,
        VerticalContentAlignment = VerticalAlignment.Center,
        HorizontalContentAlignment = HorizontalAlignment.Stretch,
        Items =
        {
          _imageView,
          new StackLayoutItem(CreateButtonPanel(), true),
        },
      };
    }

    private Panel CreateButtonPanel()
    {
      return new StackLayout()
      {
        Orientation = Orientation.Horizontal,
        VerticalContentAlignment = VerticalAlignment.Center,
        HorizontalContentAlignment = HorizontalAlignment.Center,
        Items =
        {
          new StackLayoutItem(CreateSaveButton(), true),
          new StackLayoutItem(_gameStatusLabel, true),
          new StackLayoutItem(CreateNextButton(), true),
        },
      };
    }

    private Button CreateSaveButton()
    {
      var button = new Button()
      {
        Text = "Save",
        Font = new Font(FontFamilies.Sans, TextFontSize),
        Height = ButtonHeight,
      };

      button.Click += (_, _) => ChooseFile();
      return button;
    }

    private Button CreateNextButton()
    {
      var button = new Button()
      {
        Text = "Next",
        Font = new Font(FontFamilies.Sans, TextFontSize),
        Height = ButtonHeight,
      };

      button.Click += (_, _) => _events.OnNext();

      return button;
    }

    private Label CreateGameStatusLabel()
    {
      return new Label()
      {
        Text = "Game Status: Unknown",
        TextAlignment = TextAlignment.Center,
        Font = new Font(FontFamilies.Sans, TextFontSize),
      };
    }

    private void ChooseFile()
    {
      var dialog = new SaveFileDialog();
      if (dialog.ShowDialog(this) == DialogResult.Ok)
      {
        _events.OnSave(dialog.FileName);
      }
    }
  }
}