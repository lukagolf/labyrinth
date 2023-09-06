using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using LanguageExt;
using Newtonsoft.Json.Linq;
using Players;

namespace JsonUtilities
{
  internal static class JsonConverterHelpers
  {
    
    public static IPlayerState ToPlayerState(JObject jObject, bool distinctHomes)
    {
      IBoard board = ToBoard(jObject["board"]!);
      ITile spare = ToTile(jObject["spare"]!);
      IList<IImmutablePublicPlayerInfo> players = ParsePlayers(jObject["plmt"]!);
      Option<ISlideAction> lastSlide = ToSlideAction(jObject["last"]!);
      return new PlayerState(players, board, spare, lastSlide, distinctHomes);
    }

    public static JToken ToJson(Option<IPlayerState> maybeState)
    {
      return maybeState
        .Some(ToJson)
        .None(false);
    }

    public static Player ToPlayer(JArray array)
    {
      string name = array[0].ToObject<string>()!;
      StrategyTypeJson strategyType = array[1].ToObject<StrategyTypeJson>();
      IPlayerStrategy strategy = ToStrategy(strategyType);
      return new Player(name, strategy, new BoardBuilder(), new RuleBook());
    }

    public static IBoard ToBoard(JToken jToken)
    {
      var boardJson = jToken.ToObject<BoardJson>()!;
      var tileMatrix = new ITile[boardJson.Connectors.GetLength(0), boardJson.Connectors.GetLength(1)];
      tileMatrix.Fill((rowIdx, colIdx) =>
      {
        string connector = boardJson.Connectors[rowIdx, colIdx];
        IList<string> gems = boardJson.Treasures[rowIdx, colIdx];
        string gemName1 = gems[0];
        string gemName2 = gems[1];
        return CreateTile(connector, gemName1, gemName2);
      });
      return new Board(tileMatrix);
    }

    public static JToken ToJson(IImmutableBoard board)
    {
      string[,] connectors = new string[board.GetHeight(), board.GetWidth()];
      IList<string>[,] treasures = new IList<string>[board.GetHeight(), board.GetWidth()];

      connectors.Fill((rowIdx, colIdx) =>
      {
        ITile tile = board.GetTileAt(new BoardPosition(rowIdx, colIdx));
        return ToConnector(tile);
      });

      treasures.Fill((rowIdx, colIdx) =>
      {
        ITile tile = board.GetTileAt(new BoardPosition(rowIdx, colIdx));
        return ToList(tile.Treasure);
      });

      BoardJson boardJson = new BoardJson(connectors, treasures);
      return JObject.FromObject(boardJson);
    }

    public static JToken ToJson(ITile tile)
    {
      string connector = ToConnector(tile);
      string gem1 = CamelCaseToLowerCase(tile.Treasure.Gem1.ToString());
      string gem2 = CamelCaseToLowerCase(tile.Treasure.Gem2.ToString());

      var jObject = new JObject();
      jObject["tilekey"] = connector;
      jObject["1-image"] = gem1;
      jObject["2-image"] = gem2;

      return jObject;
    }

    public static BoardPosition ToBoardPosition(CoordinateJson coordinateJson)
    {
      return new BoardPosition(coordinateJson.Row, coordinateJson.Column);
    }

    public static CoordinateJson ToCoordinate(BoardPosition boardPosition)
    {
      return new CoordinateJson(boardPosition.RowIndex, boardPosition.ColumnIndex);
    }

    public static Option<ISlideAction> ToSlideAction(JToken jToken)
    {
      if (jToken.Type == JTokenType.Null)
      {
        return Option<ISlideAction>.None;
      }

      int index = jToken[0]!.ToObject<int>();
      SlideType slideType = ToSlideType(jToken[1]!);
      return new SlideAction(slideType, index);
    }

    public static JToken ToJson(Option<ISlideAction> maybeAction)
    {
      return maybeAction
        .Some(action => (JToken) new JArray
        {
          action.Index,
          ToJson(action.Type),
        })
        .None(JValue.CreateNull);
    }

    public static JToken ToJson(IPlayerState state)
    {
      return new JObject
      {
        ["board"] = ToJson(state.Board),
        ["spare"] = ToJson(state.SpareTile),
        ["plmt"] = ToJson(state.AllPlayers.ToList()),
        ["last"] = ToJson(state.LastSlideAction)
      };
    }

    public static IPlayerStrategy ToStrategy(StrategyTypeJson strategyType)
    {
      return strategyType switch
      {
        StrategyTypeJson.Riemann => new RiemannStrategy(),
        StrategyTypeJson.Euclid => new EuclidStrategy(),
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    public static SetupCall ToSetupCall(JArray array, bool distinctHomes)
    {
      Option<IPlayerState> state = ToMaybePlayerState(array[0], distinctHomes);
      BoardPosition goal = ToBoardPosition(array[1].ToObject<CoordinateJson>()!);
      return new SetupCall(state, goal);
    }

    public static TakeTurnCall ToTakeTurnCall(JArray array, bool distinctHomes)
    {
      IPlayerState state = ToPlayerState((JObject) array[0], distinctHomes);
      return new TakeTurnCall(state);
    }

    public static WonCall ToWonCall(JArray array)
    {
      bool won = array[0].ToObject<bool>();
      return new WonCall(won);
    }
    
    public static JValue ToJson(SlideType slideType)
    {
      return JValue.CreateString(
        slideType switch
        {
          SlideType.SlideRowLeft => "LEFT",
          SlideType.SlideRowRight => "RIGHT",
          SlideType.SlideColumnUp => "UP",
          SlideType.SlideColumnDown => "DOWN",
          _ => throw new ArgumentOutOfRangeException(nameof(slideType))
        }
      );
    }

    public static SlideType ToSlideType(JToken value)
    {
      string direction = value.ToObject<string>()!;
      return direction switch
      {
        "LEFT" => SlideType.SlideRowLeft,
        "RIGHT" => SlideType.SlideRowRight,
        "UP" => SlideType.SlideColumnUp,
        "DOWN" => SlideType.SlideColumnDown,
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
      };
    }
    
    public static ITile ToTile(JToken jToken)
    {
      string connector = jToken["tilekey"]!.ToObject<string>()!;
      string gemName1 = jToken["1-image"]!.ToObject<string>()!;
      string gemName2 = jToken["2-image"]!.ToObject<string>()!;
      return CreateTile(connector, gemName1, gemName2);
    }

    private static Option<IPlayerState> ToMaybePlayerState(JToken jToken, bool distinctHomes)
    {
      return jToken.Type switch
      {
        JTokenType.Boolean => Option<IPlayerState>.None,
        JTokenType.Object => Option<IPlayerState>.Some(ToPlayerState((JObject) jToken, distinctHomes)),
        _ => throw new ArgumentOutOfRangeException(nameof(jToken))
      };
    }

    private static ITile CreateTile(string connector, string gemName1, string gemName2)
    {
      ITreasure treasure = CreateTreasure(gemName1, gemName2);
      return connector switch
      {
        "│" => new Tile(true, false, true, false, treasure),
        "─" => new Tile(false, true, false, true, treasure),
        "┐" => new Tile(false, true, true, false, treasure),
        "└" => new Tile(true, false, false, true, treasure),
        "┌" => new Tile(false, false, true, true, treasure),
        "┘" => new Tile(true, true, false, false, treasure),
        "┬" => new Tile(false, true, true, true, treasure),
        "├" => new Tile(true, false, true, true, treasure),
        "┴" => new Tile(true, true, false, true, treasure),
        "┤" => new Tile(true, true, true, false, treasure),
        "┼" => new Tile(true, true, true, true, treasure),
        _ => throw new ArgumentException("Unknown connector")
      };
    }

    private static IList<IImmutablePublicPlayerInfo> ParsePlayers(JToken plmt)
    {
      IList<PublicPlayerInfoJson> playerInfoJsonList = plmt.ToObject<IList<PublicPlayerInfoJson>>()!;
      return playerInfoJsonList
        .Select(ToPublicPlayerInfo)
        .ToList();
    }

    private static IImmutablePublicPlayerInfo ToPublicPlayerInfo(PublicPlayerInfoJson playerInfoJson)
    {
      return new PublicPlayerInfo(
        Color.Create(playerInfoJson.Color),
        ToBoardPosition(playerInfoJson.Home),
        ToBoardPosition(playerInfoJson.Current)
      );
    }

    private static JToken ToJson(IList<IImmutablePublicPlayerInfo> players)
    {
      return JArray.FromObject(
        players.Select(ToPlayerInfoJson).ToList()
      );
    }

    private static PublicPlayerInfoJson ToPlayerInfoJson(IImmutablePublicPlayerInfo player)
    {
      return new PublicPlayerInfoJson(
        ToCoordinate(player.CurrentPosition),
        ToCoordinate(player.HomePosition),
        player.Color.ToString()
      );
    }

    private static string ToConnector(ITile tile)
    {
      return ConnectorDict[ToConnections(tile)];
    }

    private static ITreasure CreateTreasure(string gemName1, string gemName2)
    {
      Gem gem1 = CreateGem(gemName1);
      Gem gem2 = CreateGem(gemName2);
      return new Treasure(gem1, gem2);
    }

    private static Gem CreateGem(string gemName)
    {
      string newGemName = LowerCaseToCamelCase(gemName);
      return (Gem) Enum.Parse(typeof(Gem), newGemName);
    }

    private static string LowerCaseToCamelCase(string gem)
    {
      var builder = new StringBuilder();
      for (int i = 0; i < gem.Length; i += 1)
      {
        if (i == 0)
        {
          builder.Append(char.ToUpper(gem[i]));
          continue;
        }

        if (gem[i].Equals('-'))
        {
          i += 1;
          builder.Append(char.ToUpper(gem[i]));
          continue;
        }

        builder.Append(gem[i]);
      }

      return builder.ToString();
    }

    private static IList<string> ToList(ITreasure treasure)
    {
      string gem1 = CamelCaseToLowerCase(treasure.Gem1.ToString());
      string gem2 = CamelCaseToLowerCase(treasure.Gem2.ToString());
      return new List<string> {gem1, gem2};
    }

    private static string CamelCaseToLowerCase(string gem)
    {
      var builder = new StringBuilder().Append(char.ToLower(gem[0]));
      foreach (char c in gem.Substring(1))
      {
        if (char.IsUpper(c))
        {
          builder.Append("-");
        }

        builder.Append(char.ToLower(c));
      }

      return builder.ToString();
    }

    private static readonly IDictionary<Connections, string> ConnectorDict = new Dictionary<Connections, string>
    {
      [new(true, false, true, false)] = "│",
      [new(false, true, false, true)] = "─",
      [new(false, true, true, false)] = "┐",
      [new(true, false, false, true)] = "└",
      [new(false, false, true, true)] = "┌",
      [new(true, true, false, false)] = "┘",
      [new(false, true, true, true)] = "┬",
      [new(true, false, true, true)] = "├",
      [new(true, true, false, true)] = "┴",
      [new(true, true, true, false)] = "┤",
      [new(true, true, true, true)] = "┼",
    };

    private static Connections ToConnections(ITile tile)
    {
      return new Connections(
        tile.HasUpConnection(),
        tile.HasLeftConnection(),
        tile.HasDownConnection(),
        tile.HasRightConnection()
      );
    }

    private readonly struct Connections
    {
      // ReSharper disable once NotAccessedField.Local
      private readonly bool _up;

      // ReSharper disable once NotAccessedField.Local
      private readonly bool _left;

      // ReSharper disable once NotAccessedField.Local
      private readonly bool _down;

      // ReSharper disable once NotAccessedField.Local
      private readonly bool _right;

      public Connections(bool up, bool left, bool down, bool right)
      {
        _up = up;
        _left = left;
        _down = down;
        _right = right;
      }
    }
  }
}