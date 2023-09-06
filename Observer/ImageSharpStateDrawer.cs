using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;
using LanguageExt;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;
using PlayerColor = Common.Color;
using Image = SixLabors.ImageSharp.Image;
using Path = System.IO.Path;

namespace Observer
{
  /// <summary>
  /// Provides an IStateDrawer implementation that can draw the given game state as an ImageSharp (C# 2D image library)
  /// image
  /// </summary>
  internal sealed class ImageSharpStateDrawer : IStateDrawer<Image>
  {
    private const int UnitSize = 3;
    private const int ExtraColumnsForSpareTile = 2;
    private static readonly Color CanvasBackground = new(new Rgb24(174,168,182));
    private static readonly Color BoardBackground = new(new Rgb24(140,110,76));
    private static readonly Color TileBackground = new(new Rgb24(140,110,76));
    private static readonly Color LastSlideBackground = new(new Rgb24(153,154,166));
    private static readonly Color MarkColor = new(new Rgb24(201,186,167));
    private static readonly Color OutlineColor = Color.Black;
    private static readonly Color TextColor = Color.Black;
    private const float OutlineThickness = 1f;
    private const int FontSize = 16;
    private readonly Font _font;
    private readonly IDictionary<Gem, Image> _gemImages;

    public ImageSharpStateDrawer(FileInfo fontFile, DirectoryInfo gemImageDir)
    {
      FontCollection collection = new();
      FontFamily family = collection.Add(fontFile.FullName);
      _font = family.CreateFont(FontSize, FontStyle.Regular);
      _gemImages = LoadGemImages(gemImageDir);
    }

    public Image Draw(IRefereeState state, int maxPixelWidth, int maxPixelHeight)
    {
      IImmutableBoard board = state.Board;
      int boardWidth = board.GetWidth();
      int boardHeight = board.GetHeight();
      int tileLengthCandidate1 = CalcTileLength(maxPixelWidth, boardWidth + ExtraColumnsForSpareTile);
      int tileLengthCandidate2 = CalcTileLength(maxPixelHeight, boardHeight);
      int tileLength = Math.Min(tileLengthCandidate1, tileLengthCandidate2);
      return DrawState(state, tileLength);
    }

    private Image DrawState(IRefereeState state, int tileLength)
    {
      IImmutableBoard board = state.Board;
      int canvasWidth = tileLength * (board.GetWidth() + ExtraColumnsForSpareTile);
      int canvasHeight = board.GetHeight() * tileLength;
      Image canvas = DrawRectangle(canvasWidth, canvasHeight, CanvasBackground);
      var homePositions = ExtractHomePositions(state);
      var currentPositions = ExtractCurrentPositions(state);
      Image boardImage = DrawBoard(board, tileLength, homePositions, currentPositions);
      Image spareTileImage = DrawTile(state.SpareTile, tileLength, Option<Color>.None, Option<Color>.None);
      Image lastSlideImage = DrawLastSlide(state.LastSlideAction, tileLength);

      int x = (board.GetWidth() * tileLength) + (tileLength / 2);
      int y = ((canvasHeight - tileLength) / 2);
      int yOffset = canvasHeight / 3;

      PlaceOnCanvas(canvas, boardImage, 0, 0);
      PlaceOnCanvas(canvas, spareTileImage, x, y - yOffset);
      PlaceOnCanvas(canvas, lastSlideImage, x, y + yOffset);

      return canvas;
    }

    private static int CalcTileLength(int canvasLength, int numberOfTiles)
    {
      return (canvasLength / numberOfTiles / UnitSize) * UnitSize;
    }

    private Image DrawBoard(IImmutableBoard board, int tileLength, IDictionary<BoardPosition, Color> homePositions,
      IDictionary<BoardPosition, Color> currentPositions)
    {
      int boardPixelHeight = board.GetHeight() * tileLength;
      int boardPixelWidth = board.GetWidth() * tileLength;
      Image img = DrawRectangle(boardPixelWidth, boardPixelHeight, BoardBackground);
      img.Mutate(context =>
      {
        board.Positions.ForEach(posn =>
        {
          Option<Color> homeColor = homePositions.ContainsKey(posn) ? homePositions[posn] : Option<Color>.None;
          Option<Color> currentColor = currentPositions.ContainsKey(posn) ? currentPositions[posn] : Option<Color>.None;
          ITile tile = board.GetTileAt(posn);
          Image tileImg = DrawTile(tile, tileLength, homeColor, currentColor);
          int x = posn.ColumnIndex * tileLength;
          int y = posn.RowIndex * tileLength;
          context.DrawImage(tileImg, new Point(x, y), 1f);
        });
      });
      return img;
    }

    private static void PlaceOnCanvas(Image canvas, Image imageToPlace, int x, int y)
    {
      canvas.Mutate(context => { context.DrawImage(imageToPlace, new Point(x, y), 1f); });
    }

    private static Image DrawRectangle(int pixelWidth, int pixelHeight, Color backgroundColor)
    {
      Image img = new Image<Rgb24>(pixelWidth, pixelHeight);
      img.Mutate(context => context.Fill(backgroundColor));
      return img;
    }

    private Image DrawTile(ITile tile, int tileLength, Option<Color> home, Option<Color> current)
    {
      int unitLength = tileLength / UnitSize;
      Image tileImage = DrawEmptyTile(tileLength);
      DrawTileCenter(tileImage, unitLength);
      DrawTileUpConnection(tile, tileImage, unitLength);
      DrawTileLeftConnection(tile, tileImage, unitLength);
      DrawTileDownConnection(tile, tileImage, unitLength);
      DrawTileRightConnection(tile, tileImage, unitLength);
      DrawTreasureOnTile(tileImage, tile.Treasure, unitLength);
      home.IfSome(color => DrawHomeOnTile(tileImage, color, unitLength));
      current.IfSome(color => DrawAvatarOnTile(tileImage, color, unitLength));
      return tileImage;
    }

    private void DrawTreasureOnTile(Image tileImage, ITreasure treasure, int unitLength)
    {
      int gemSize = (int) (0.75 * unitLength);
      Image gemImage1 = _gemImages[treasure.Gem1].Clone(context => context.Resize(gemSize, gemSize));
      Image gemImage2 = _gemImages[treasure.Gem2].Clone(context => context.Resize(gemSize, gemSize));
      int gem1X = (int) (0.25 * unitLength);
      int gem1Y = (int) (0.25 * unitLength);
      int gem2X = 2 * unitLength;
      int gem2Y = 2 * unitLength;
      tileImage.Mutate(context =>
      {
        context.DrawImage(gemImage1, new Point(gem1X, gem1Y), 1f);
        context.DrawImage(gemImage2, new Point(gem2X, gem2Y), 1f);
      });
    }

    private Image DrawLastSlide(Option<ISlideAction> lastSlide, int tileLength)
    {
      string slideDescription = lastSlide
        .Some(slide => ToDirection(slide.Type) + " " + slide.Index)
        .None(() => "No last slide");

      string text = "Last Slide:\n" + slideDescription;
      Image lastSlideImage = DrawRectangle(tileLength, tileLength, LastSlideBackground);
      lastSlideImage.Mutate(context => { context.DrawText(text, _font, TextColor, new PointF(5, 5)); });
      return lastSlideImage;
    }

    private static string ToDirection(SlideType slideType)
    {
      return slideType switch
      {
        SlideType.SlideColumnUp => "UP",
        SlideType.SlideRowLeft => "LEFT",
        SlideType.SlideColumnDown => "DOWN",
        SlideType.SlideRowRight => "RIGHT",
        _ => throw new ArgumentOutOfRangeException(nameof(slideType))
      };
    }

    private static Image DrawEmptyTile(int tileLength)
    {
      var outLine = new Rectangle(0, 0, tileLength, tileLength);
      Image img = new Image<Rgb24>(tileLength, tileLength);
      img.Mutate(context =>
      {
        context.Fill(TileBackground);
        context.Draw(OutlineColor, OutlineThickness, outLine);
      });
      return img;
    }

    private static void DrawTileCenter(Image emptyTileImg, int unitLength)
    {
      var rect = new Rectangle(unitLength, unitLength, unitLength, unitLength);
      emptyTileImg.Mutate(context => context.Fill(MarkColor, rect));
    }

    private static void DrawTileUpConnection(ITile tile, Image baseTileImg, int unitLength)
    {
      if (!tile.HasUpConnection())
      {
        return;
      }

      var rect = new Rectangle(unitLength, 0, unitLength, unitLength);
      baseTileImg.Mutate(context => context.Fill(MarkColor, rect));
    }

    private static void DrawTileLeftConnection(ITile tile, Image baseTileImg, int unitLength)
    {
      if (!tile.HasLeftConnection())
      {
        return;
      }

      var rect = new Rectangle(0, unitLength, unitLength, unitLength);
      baseTileImg.Mutate(context => context.Fill(MarkColor, rect));
    }

    private static void DrawTileDownConnection(ITile tile, Image baseTileImg, int unitLength)
    {
      if (!tile.HasDownConnection())
      {
        return;
      }

      var rect = new Rectangle(unitLength, 2 * unitLength, unitLength, unitLength);
      baseTileImg.Mutate(context => context.Fill(MarkColor, rect));
    }

    private static void DrawTileRightConnection(ITile tile, Image baseTileImg, int unitLength)
    {
      if (!tile.HasRightConnection())
      {
        return;
      }

      var rect = new Rectangle(2 * unitLength, unitLength, unitLength, unitLength);
      baseTileImg.Mutate(context => context.Fill(MarkColor, rect));
    }

    private static void DrawHomeOnTile(Image tileImage, Color color, int unitLength)
    {
      int x = 0;
      int y = 2 * unitLength;
      var rect = new Rectangle(x, y, unitLength, unitLength);
      tileImage.Mutate(context => context.Fill(color, rect));
    }
    
    private static void DrawAvatarOnTile(Image tileImage, Color color, int unitLength)
    {
      int radius = unitLength / 2;
      int x = (2 * unitLength) + radius;
      int y = radius;
      var circle = new EllipsePolygon(x, y, radius);
      tileImage.Mutate(context => context.Fill(color, circle));
    }

    private static string CamelCaseToLowerCase(string gem)
    {
      var builder = new StringBuilder().Append(char.ToLower(gem[0]));
      foreach (char c in gem.Substring(1))
      {
        if (char.IsUpper(c))
        {
          builder.Append("-");
        }

        builder.Append(char.ToLower(c));
      }

      return builder.ToString();
    }

    private static IDictionary<Gem, Image> LoadGemImages(DirectoryInfo gemImageDir)
    {
      string dir = gemImageDir.FullName;
      return Enum.GetValues<Gem>()
        .ToDictionary(
          gem => gem,
          gem =>
          {
            string gemName = CamelCaseToLowerCase(gem.ToString());
            string gemFile = Path.ChangeExtension(gemName, ".png");
            string fullPath = Path.Combine(dir, gemFile);
            return Image.Load(fullPath);
          }
        );
    }

    private static IDictionary<BoardPosition, Color> ExtractHomePositions(IRefereeState state)
    {
      var dict = new Dictionary<BoardPosition, Color>();
      state.AllPlayers.ForEach(p =>
      {
        dict[p.HomePosition] = ConvertColor(p.Color);
      });

      return dict;
    }

    private static IDictionary<BoardPosition, Color> ExtractCurrentPositions(IRefereeState state)
    {
      var dict = new Dictionary<BoardPosition, Color>();
      state.AllPlayers.ForEach(p =>
      {
        dict[p.CurrentPosition] = ConvertColor(p.Color);
      });

      return dict;
    }

    private static Color ConvertColor(PlayerColor color)
    {
      return new Color(new Rgb24(color.RedValue, color.GreenValue, color.BlueValue));
    }
  }
}