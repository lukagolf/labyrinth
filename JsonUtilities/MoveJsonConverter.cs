using System;
using Common;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Players;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class MoveJsonConverter : JsonConverter<Either<IMove, Pass>>
  {
    public override void WriteJson(JsonWriter writer, Either<IMove, Pass> value, JsonSerializer serializer)
    {
      JToken token = ToJson(value);
      token.WriteTo(writer);
    }

    public override Either<IMove, Pass> ReadJson(JsonReader reader, Type objectType, Either<IMove, Pass> existingValue,
      bool hasExistingValue,
      JsonSerializer serializer)
    {
      var token = JToken.Load(reader);
      return ToMaybeMove(token); 
    }

    private static JToken ToJson(Either<IMove, Pass> maybeMove)
    {
      return maybeMove.Match(
        _ => JValue.CreateString("PASS"),
        move => (JToken) ToJson(move)
      );
    }

    private static Either<IMove, Pass> ToMaybeMove(JToken token)
    {
      if (token.Type == JTokenType.String && token.ToObject<string>()!.Equals("PASS"))
      {
        return Either<IMove, Pass>.Right(Pass.Value);
      }

      return Either<IMove, Pass>.Left(ToMove((JArray) token));
    }

    private static JArray ToJson(IMove move)
    {
      return new JArray
      {
        move.Slide.Index,
        JsonConverterHelpers.ToJson(move.Slide.Type),
        ToJson(move.Rotation),
        JToken.FromObject(ToCoordinate(move.Destination)),
      };
    }

    private static IMove ToMove(JArray array)
    {
      int index = array[0].ToObject<int>();
      SlideType slideType = ToSlideType(array[1]);
      Rotation rotation = ToRotation(array[2]);
      BoardPosition destination = ToBoardPosition(array[3].ToObject<CoordinateJson>()!);
      return new Move(new SlideAction(slideType, index), rotation, destination);
    }

    private static JToken ToJson(Rotation rotation)
    {
      int cwDegrees = rotation switch
      {
        Rotation.Zero => 0,
        Rotation.Ninety => 90,
        Rotation.OneHundredEighty => 180,
        Rotation.TwoHundredSeventy => 270,
        _ => throw new ArgumentOutOfRangeException(nameof(rotation))
      };

      return ConvertDegrees(cwDegrees);
    }

    private static Rotation ToRotation(JToken token)
    {
      int ccwDegrees = token.ToObject<int>();
      int cwDegrees = ConvertDegrees(ccwDegrees);
      return cwDegrees switch
      {
        0 => Rotation.Zero,
        90 => Rotation.Ninety,
        180 => Rotation.OneHundredEighty,
        270 => Rotation.TwoHundredSeventy,
        _ => throw new ArgumentOutOfRangeException(nameof(cwDegrees))
      };
    }

    private static int ConvertDegrees(int degrees)
    {
      return (360 - degrees) % 360;
    }
  }
}