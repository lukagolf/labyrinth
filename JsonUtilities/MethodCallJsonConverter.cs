using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneOf;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public class MethodCallJsonConverter : JsonConverter<OneOf<SetupCall, TakeTurnCall, WonCall>>
  {
    private readonly bool _distinctHomes;

    public MethodCallJsonConverter(bool distinctHomes = true)
    {
      _distinctHomes = distinctHomes;
    }

    public override void WriteJson(JsonWriter writer, OneOf<SetupCall, TakeTurnCall, WonCall> value,
      JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override OneOf<SetupCall, TakeTurnCall, WonCall> ReadJson(JsonReader reader, Type objectType,
      OneOf<SetupCall, TakeTurnCall, WonCall> existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      var jArray = JArray.Load(reader);
      string methodName = jArray[0].ToObject<string>()!;
      return methodName switch
      {
        "setup" => ToSetupCall((JArray) jArray[1], _distinctHomes),
        "take-turn" => ToTakeTurnCall((JArray) jArray[1], _distinctHomes),
        "win" => ToWonCall((JArray) jArray[1]),
        _ => throw new ArgumentOutOfRangeException(nameof(methodName)),
      };
    }
  }
}