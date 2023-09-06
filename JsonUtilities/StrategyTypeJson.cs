using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonUtilities
{
  public enum StrategyTypeJson
  {
    [JsonProperty("Euclid")] [JsonConverter(typeof(StringEnumConverter))]
    Euclid,

    [JsonProperty("Riemann")] [JsonConverter(typeof(StringEnumConverter))]
    Riemann,
  }
}