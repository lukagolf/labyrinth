using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Players;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class PlayerJsonConverter : JsonConverter<IPlayer>
  {
    public override void WriteJson(JsonWriter writer, IPlayer? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override IPlayer ReadJson(JsonReader reader, Type objectType, IPlayer? existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      var array = JArray.Load(reader);
      return ToPlayer(array);
    }
  }
}