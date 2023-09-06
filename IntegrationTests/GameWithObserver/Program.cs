using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Common;
using Eto.Drawing;
using JsonUtilities;
using Newtonsoft.Json;
using Observer;
using Players;
using Referee;

namespace GameWithObserver
{
  public static class Program
  {
    private const int Width = 1800;
    private const int Height = 900;

    public static void Main()
    {
      var testInputs = ReadTestInputs(Console.In);
      RunTest(testInputs.players, testInputs.state);
    }

    private static (IList<IPlayer> players, IRefereeState state) ReadTestInputs(TextReader reader)
    {
      var jsonReader = new JsonTextReader(reader) {SupportMultipleContent = true};
      JsonSerializer serializer = CreateSerializer();
      IList<IPlayer> players = ReadPlayers(jsonReader, serializer);
      IRefereeState state = ReadState(jsonReader, serializer);
      return (players, state);
    }

    private static void RunTest(IList<IPlayer> players, IRefereeState state)
    {
      IRunnableObserver observer = CreateGuiObserver();
      IReferee referee = new Referee.Referee(new RuleBook(), 10000);
      Task.Run(() => referee.RunGame(state, players, new List<IObserver> {observer}));
      observer.Run();
    }

    private static IRunnableObserver CreateGuiObserver()
    {
      var fontFile = new FileInfo("./../Maze/Assets/Fonts/OpenSans-Regular.ttf");
      var gemImageDir = new DirectoryInfo("./../Maze/Assets/Images/Gems");
      IStateDrawer<Image> stateDrawer = new EtoStateDrawer(fontFile, gemImageDir);
      IObserverModel model = new ObserverModel();
      return new GuiObserver(model, stateDrawer, Width, Height);
    }

    private static IList<IPlayer> ReadPlayers(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<IList<IPlayer>>(reader)!;
    }

    private static IRefereeState ReadState(JsonReader reader, JsonSerializer serializer)
    {
      reader.Read();
      return serializer.Deserialize<IRefereeState>(reader)!;
    }

    private static JsonSerializer CreateSerializer()
    {
      var serializer = new JsonSerializer();
      serializer.Converters.Add(new MixedPlayerJsonConverter());
      serializer.Converters.Add(new RefereeStateJsonConverter(false));
      return serializer;
    }
  }
}