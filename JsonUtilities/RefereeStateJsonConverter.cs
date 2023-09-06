using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static JsonUtilities.JsonConverterHelpers;

namespace JsonUtilities
{
  public sealed class RefereeStateJsonConverter : JsonConverter<IRefereeState>
  {
    private readonly bool _distinctHomes;

    public RefereeStateJsonConverter(bool distinctHomes = true)
    {
      _distinctHomes = distinctHomes;
    }

    public override void WriteJson(JsonWriter writer, IRefereeState? value, JsonSerializer serializer)
    {
      var jObject = new JObject
      {
        ["board"] = JsonConverterHelpers.ToJson(value!.Board),
        ["spare"] = JsonConverterHelpers.ToJson(value.SpareTile),
        ["plmt"] = ToJson(value.AllPlayers, value.Board),
        ["last"] = JsonConverterHelpers.ToJson(value.LastSlideAction)
      };
      jObject.WriteTo(writer);
    }

    public override IRefereeState ReadJson(JsonReader reader, Type objectType, IRefereeState? existingValue,
      bool hasExistingValue,
      JsonSerializer serializer)
    {
      var jObject = JObject.Load(reader);
      return ToRefereeState(jObject, _distinctHomes);
    }

    private static IRefereeState ToRefereeState(JObject jObject, bool distinctHomes)
    {
      IBoard board = ToBoard(jObject["board"]!);
      ITile spare = ToTile(jObject["spare"]!);
      Option<ISlideAction> lastSlide = ToSlideAction(jObject["last"]!);
      IList<IPlayerInfo> players = ParsePlayers(jObject["plmt"]!, board);
      IList<ITreasure> treasures = jObject.ContainsKey("goals")
        ? CreateTreasures(jObject["goals"]!.ToObject<IEnumerable<CoordinateJson>>()!, board)
        : Array.Empty<ITreasure>();
      return new RefereeState(players, board, spare, lastSlide, treasures, distinctHomes);
    }

    private static IList<IPlayerInfo> ParsePlayers(JToken plmt, IImmutableBoard board)
    {
      IList<PlayerInfoJson> playerInfoJsonList = plmt.ToObject<IList<PlayerInfoJson>>()!;
      return playerInfoJsonList
        .Select(playerInfoJson => ToPlayerInfo(playerInfoJson, board))
        .ToList();
    }

    private static IPlayerInfo ToPlayerInfo(PlayerInfoJson playerInfoJson, IImmutableBoard board)
    {
      var goal = ToBoardPosition(playerInfoJson.Goal);
      var treasure = board.GetTileAt(goal).Treasure;
      return new PlayerInfo(
        Color.Create(playerInfoJson.Color),
        ToBoardPosition(playerInfoJson.Home),
        ToBoardPosition(playerInfoJson.Current),
        treasure
      );
    }

    private static IList<ITreasure> CreateTreasures(IEnumerable<CoordinateJson> coordinates, IImmutableBoard board)
    {
      return coordinates.Select(c => board.GetTileAt(ToBoardPosition(c)).Treasure).ToList();
    }

    private static JToken ToJson(IEnumerable<IPlayerInfo> playerInfos, IImmutableBoard board)
    {
      return JToken.FromObject(
        playerInfos
          .Select(playerInfo => ToJson(playerInfo, board))
          .ToList()
      );
    }

    private static PlayerInfoJson ToJson(IPlayerInfo playerInfo, IImmutableBoard board)
    {
      var goal = board.Positions
        .First(p => board.GetTileAt(p).Treasure.Equals(playerInfo.CurrentlyAssignedTreasure));
      return new PlayerInfoJson(
        ToCoordinate(playerInfo.CurrentPosition),
        ToCoordinate(playerInfo.HomePosition),
        ToCoordinate(goal),
        playerInfo.Color.ToString()
      );
    }
  }
}