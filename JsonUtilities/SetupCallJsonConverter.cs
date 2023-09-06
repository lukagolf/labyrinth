using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class SetupCallJsonConverter : JsonConverter<SetupCall>
  {
    private readonly bool _distinctHomes;

    public SetupCallJsonConverter(bool distinctHomes = true)
    {
      _distinctHomes = distinctHomes;
    }

    public override void WriteJson(JsonWriter writer, SetupCall? value, JsonSerializer serializer)
    {
      var jArray = new JArray(
        "setup",
        new JArray(
          ToJson(value!.State),
          JObject.FromObject(ToCoordinate(value.Goal))
        )
      );

      jArray.WriteTo(writer);
    }

    public override SetupCall ReadJson(JsonReader reader, Type objectType, SetupCall? existingValue,
      bool hasExistingValue,
      JsonSerializer serializer)
    {
      var jArray = JArray.Load(reader);
      return ToSetupCall((JArray) jArray[1], _distinctHomes);
    }
  }
}