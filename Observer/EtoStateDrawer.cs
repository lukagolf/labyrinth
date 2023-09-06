using System.IO;
using Common;
using Eto.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using EtoImage = Eto.Drawing.Image;

namespace Observer
{
  public sealed class EtoStateDrawer : IStateDrawer<EtoImage>
  {
    private readonly IStateDrawer<ImageSharpImage> _stateDrawer;

    public EtoStateDrawer(FileInfo fontFile, DirectoryInfo gemImageDir)
    {
      _stateDrawer = new ImageSharpStateDrawer(fontFile, gemImageDir);
    }

    public EtoImage Draw(IRefereeState state, int maxPixelWidth, int maxPixelHeight)
    {
      ImageSharpImage image = _stateDrawer.Draw(state, maxPixelWidth, maxPixelHeight);
      var memory = new MemoryStream();
      image.Save(memory, new JpegEncoder());
      return new Bitmap(memory.ToArray());
    }
  }
}