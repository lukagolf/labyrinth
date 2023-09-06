using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonUtilities
{
  public enum BadTypeJson
  {
    [JsonProperty("setUp")] [JsonConverter(typeof(StringEnumConverter))]
    Setup,
    
    [JsonProperty("takeTurn")] [JsonConverter(typeof(StringEnumConverter))]
    TakeTurn,
    
    [JsonProperty("win")] [JsonConverter(typeof(StringEnumConverter))]
    Win,
  }
}