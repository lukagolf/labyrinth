using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class SlideTypeJsonConverter : JsonConverter<SlideType>
  {
    public override void WriteJson(JsonWriter writer, SlideType value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override SlideType ReadJson(JsonReader reader, Type objectType, SlideType existingValue,
      bool hasExistingValue, JsonSerializer serializer)
    {
      var jToken = JToken.Load(reader);
      return ToSlideType(jToken);
    }
  }
}