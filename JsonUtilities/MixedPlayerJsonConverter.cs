using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Players;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public class MixedPlayerJsonConverter : JsonConverter<IPlayer>
  {
    public override void WriteJson(JsonWriter writer, IPlayer? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }

    public override IPlayer ReadJson(JsonReader reader, Type objectType, IPlayer? existingValue, bool hasExistingValue,
      JsonSerializer serializer)
    {
      var array = JArray.Load(reader);
      return array.Count switch
      {
        2 => ToPlayer(array),
        3 => ToBadPlayer(array),
        4 => ToBadPlayer2(array),
        _ => throw new ArgumentOutOfRangeException()
      };
    }
    
    private static BadPlayer ToBadPlayer(JArray array)
    {
      string name = array[0].ToObject<string>()!;
      StrategyTypeJson strategyType = array[1].ToObject<StrategyTypeJson>();
      IPlayerStrategy strategy = ToStrategy(strategyType);
      BadTypeJson badTypeJson = array[2].ToObject<BadTypeJson>();
      BadPlayer.BadMethod badMethod = ToBadMethod(badTypeJson);
      var badConfig = new BadPlayer.BadConfig(badMethod, BadPlayer.BadAction.ThrowException, 1);
      return new BadPlayer(name, strategy, new BoardBuilder(), badConfig);
    }
    
    private static BadPlayer ToBadPlayer2(JArray array)
    {
      string name = array[0].ToObject<string>()!;
      StrategyTypeJson strategyType = array[1].ToObject<StrategyTypeJson>();
      IPlayerStrategy strategy = ToStrategy(strategyType);
      BadTypeJson badTypeJson = array[2].ToObject<BadTypeJson>();
      BadPlayer.BadMethod badMethod = ToBadMethod(badTypeJson);
      int count = array[3].ToObject<int>();
      var badConfig = new BadPlayer.BadConfig(badMethod, BadPlayer.BadAction.TakeTooLong, count);
      return new BadPlayer(name, strategy, new BoardBuilder(), badConfig);
    }
    
    private static BadPlayer.BadMethod ToBadMethod(BadTypeJson badTypeJson)
    {
      return badTypeJson switch
      {
        BadTypeJson.Setup => BadPlayer.BadMethod.Setup,
        BadTypeJson.TakeTurn => BadPlayer.BadMethod.TakeTurn,
        BadTypeJson.Win => BadPlayer.BadMethod.Won,
        _ => throw new InvalidEnumArgumentException("Unknown bad type.")
      };
    }
  }
}