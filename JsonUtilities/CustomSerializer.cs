using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonUtilities
{
  public static class CustomSerializer
  {
    private static readonly Lazy<JsonSerializer> Lazy = new(() =>
    {
      var serializer = new JsonSerializer
      {
        Formatting = Formatting.None,
      };
      
      serializer.Converters.Add(new StringEnumConverter());
      serializer.Converters.Add(new BoardJsonConverter());
      serializer.Converters.Add(new BoardPositionJsonConverter());
      serializer.Converters.Add(new RefereeStateJsonConverter());
      serializer.Converters.Add(new PlayerStateJsonConverter());
      serializer.Converters.Add(new SlideTypeJsonConverter());
      serializer.Converters.Add(new RotationJsonConverter());
      serializer.Converters.Add(new MoveJsonConverter());
      serializer.Converters.Add(new PlayerJsonConverter());
      serializer.Converters.Add(new SetupCallJsonConverter());
      serializer.Converters.Add(new AcknowledgeJsonConverter());
      serializer.Converters.Add(new TakeTurnCallJsonConverter());
      serializer.Converters.Add(new WonCallJsonConverter());
      serializer.Converters.Add(new MethodCallJsonConverter());
      return serializer;
    });

    public static JsonSerializer Instance => Lazy.Value;
  }
}