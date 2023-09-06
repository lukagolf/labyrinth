using System.Collections.Generic;
using Newtonsoft.Json;

namespace JsonUtilities
{
  [JsonObject]
  internal sealed class BoardJson
  {
    public BoardJson(string[,] connectors, IList<string>[,] treasures)
    {
      Connectors = connectors;
      Treasures = treasures;
    }

    [JsonProperty("connectors")]
    public string[,] Connectors { get; }
    
    [JsonProperty("treasures")]
    public IList<string>[,] Treasures { get; }
  }
}