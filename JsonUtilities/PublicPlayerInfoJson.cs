using Newtonsoft.Json;

namespace JsonUtilities
{
  [JsonObject]
  internal sealed class PublicPlayerInfoJson
  {
    public PublicPlayerInfoJson(CoordinateJson current, CoordinateJson home, string color)
    {
      Current = current;
      Home = home;
      Color = color;
    }

    [JsonProperty("current")]
    public CoordinateJson Current { get; }
    
    [JsonProperty("home")]
    public CoordinateJson Home { get; }
    
    [JsonProperty("color")]
    public string Color { get; }
  }
}