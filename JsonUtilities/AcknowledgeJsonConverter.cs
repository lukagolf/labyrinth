using System;
using Common;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonUtilities
{
  public class AcknowledgeJsonConverter : JsonConverter<Acknowledge>
  {
    public override void WriteJson(JsonWriter writer, Acknowledge? value, JsonSerializer serializer)
    {
      JValue.CreateString("void").WriteTo(writer);
    }

    public override Acknowledge ReadJson(JsonReader reader, Type objectType, Acknowledge? existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      var jToken = JToken.Load(reader);
      
      if (jToken.ToObject<string>()!.Equals("void"))
      {
        return Acknowledge.Value;
      }

      throw new JsonSerializationException("Invalid acknowledgement");
    }
  }
}