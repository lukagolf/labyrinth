using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class BoardPositionJsonConverter : JsonConverter<BoardPosition>
  {
    public override void WriteJson(JsonWriter writer, BoardPosition value, JsonSerializer serializer)
    {
      CoordinateJson coordinate = ToCoordinate(value);
      JObject.FromObject(coordinate).WriteTo(writer);
    }

    public override BoardPosition ReadJson(JsonReader reader, Type objectType, BoardPosition existingValue,
      bool hasExistingValue, JsonSerializer serializer)
    {
      var jObject = JObject.Load(reader);
      var coordinate = jObject.ToObject<CoordinateJson>()!;
      return ToBoardPosition(coordinate);
    }
  }
}