using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class BoardJsonConverter : JsonConverter<IBoard>
  {
    public override void WriteJson(JsonWriter writer, IBoard? value, JsonSerializer serializer)
    {
      ToJson(value!).WriteTo(writer);
    }

    public override IBoard ReadJson(JsonReader reader, Type objectType, IBoard? existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      return ToBoard(JObject.Load(reader));
    }
  }
}