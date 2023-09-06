using System;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonUtilities
{
  public sealed class RotationJsonConverter : JsonConverter<Rotation>
  {
    public override void WriteJson(JsonWriter writer, Rotation value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override Rotation ReadJson(JsonReader reader, Type objectType, Rotation existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      int ccwDegrees = JToken.Load(reader).ToObject<int>();
      return CreateRotation(ccwDegrees);
    }
    
    private static Rotation CreateRotation(int ccwDegrees)
    {
      int cwDegrees = (360 - ccwDegrees) % 360;
      return cwDegrees switch
      {
        0 => Rotation.Zero,
        90 => Rotation.Ninety,
        180 => Rotation.OneHundredEighty,
        270 => Rotation.TwoHundredSeventy,
        _ => throw new ArgumentException("Unknown number of counter-clockwise degrees.")
      };
    }
  }
}