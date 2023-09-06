using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class PlayerStateJsonConverter : JsonConverter<IPlayerState>
  {
    private readonly bool _distinctHomes;

    public PlayerStateJsonConverter(bool distinctHomes = true)
    {
      _distinctHomes = distinctHomes;
    }

    public override void WriteJson(JsonWriter writer, IPlayerState? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override IPlayerState ReadJson(JsonReader reader, Type objectType, IPlayerState? existingValue,
      bool hasExistingValue, JsonSerializer serializer)
    {
      var jObject = JObject.Load(reader);
      return ToPlayerState(jObject, _distinctHomes);
    }
  }
}