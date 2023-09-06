using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class TakeTurnCallJsonConverter : JsonConverter<TakeTurnCall>
  {
    private readonly bool _distinctHomes;

    public TakeTurnCallJsonConverter(bool distinctHomes = true)
    {
      _distinctHomes = distinctHomes;
    }

    public override void WriteJson(JsonWriter writer, TakeTurnCall? value, JsonSerializer serializer)
    {
      var jArray = new JArray(
        "take-turn",
        new JArray(
          ToJson(value!.State)
        )
      );
      
      jArray.WriteTo(writer);
    }

    public override TakeTurnCall ReadJson(JsonReader reader, Type objectType, TakeTurnCall? existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      var jArray = JArray.Load(reader);
      return ToTakeTurnCall((JArray) jArray[1], _distinctHomes);
    }
  }
}