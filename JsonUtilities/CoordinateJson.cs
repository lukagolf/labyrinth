using Newtonsoft.Json;

namespace JsonUtilities
{
  /// <summary>
  /// Specifies a tile location
  /// </summary>
  [JsonObject]
  internal class CoordinateJson
  {
    public CoordinateJson(int row, int column)
    {
      Row = row;
      Column = column;
    }

    [JsonProperty("column#", Order = 1)] public int Column { get; }
    [JsonProperty("row#", Order = 2)] public int Row { get; }
  }
}