using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public class WonCallJsonConverter : JsonConverter<WonCall>
  {
    public override void WriteJson(JsonWriter writer, WonCall? value, JsonSerializer serializer)
    {
      var jArray = new JArray(
        "win",
        new JArray(value!.Won)
      );
      
      jArray.WriteTo(writer);
    }

    public override WonCall ReadJson(JsonReader reader, Type objectType, WonCall? existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      var jArray = JArray.Load(reader);
      return ToWonCall((JArray) jArray[1]);
    }
  }
}