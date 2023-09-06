using Newtonsoft.Json;

namespace JsonUtilities
{
  [JsonObject]
  internal sealed class PlayerInfoJson
  {
    public PlayerInfoJson(CoordinateJson current, CoordinateJson home, CoordinateJson goal, string color)
    {
      Current = current;
      Home = home;
      Goal = goal;
      Color = color;
    }

    [JsonProperty("current")] public CoordinateJson Current { get; }

    [JsonProperty("home")] public CoordinateJson Home { get; }
    
    [JsonProperty("goto")] public CoordinateJson Goal { get; }

    [JsonProperty("color")] public string Color { get; }
  }
}