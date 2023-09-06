using System.IO;

namespace JsonUtilities
{
  public sealed class CharacterReader : TextReader
  {
    private readonly TextReader _reader;

    public CharacterReader(TextReader reader)
    {
      _reader = reader;
    }

    public override void Close()
    {
      _reader.Close();
    }

    protected override void Dispose(bool disposing)
    {
      _reader.Dispose();
    }

    public override int Read(char[] buffer, int index, int count)
    {
      return _reader.Read(buffer, index, 1);
    }
  }
}