using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using JsonUtilities;
using Newtonsoft.Json;
using Players;
using Referee;
using Xunit;

namespace UnitTests.RefereeTests
{
  public sealed class SameGoalAndHomeTest
  {
    private readonly JsonSerializer _serializer;

    public SameGoalAndHomeTest()
    {
      _serializer = new JsonSerializer();
      _serializer.Converters.Add(new MixedPlayerJsonConverter());
      _serializer.Converters.Add(new RefereeStateJsonConverter(false));
    }

    [Fact]
    public void Test()
    {
      IReferee referee = new Referee.Referee(new RuleBook(), 4000);
      IRefereeState state = CreateState();
      IList<IPlayer> players = CreatePlayers();
      (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) result = referee.RunGame(state, players);
      Assert.Equal(new List<string> {"oli"}, result.winningPlayers.Select(p => p.Name));
      Assert.Equal(new List<string>{"ZENA"}, result.misbehavedPlayers.Select(p => p.Name));
    }

    private IList<IPlayer> CreatePlayers()
    {
      var reader = new JsonTextReader(new StringReader(PlayerJson));
      return _serializer.Deserialize<IList<IPlayer>>(reader)!;
    }

    private IRefereeState CreateState()
    {
      var reader = new JsonTextReader(new StringReader(StateJson));
      return _serializer.Deserialize<IRefereeState>(reader)!;
    }

    private const string PlayerJson =
      @"[[""oli"",""Riemann""],[""ZENA"",""Riemann"",""setUp"",2],[""eadam"",""Euclid""],[""ebob"",""Euclid""],[""ecarl"",""Euclid""]]";

    private const string StateJson =
      @"{""board"":{""connectors"":[[""┐"",""─"",""└"",""│"",""─"",""┐"",""└""],[""│"",""┼"",""│"",""│"",""┌"",""┘"",""┬""],[""┐"",""─"",""┌"",""│"",""├"",""┴"",""┤""],[""─"",""─"",""─"",""│"",""┼"",""│"",""─""],[""┐"",""└"",""┌"",""┘"",""┬"",""├"",""┴""],[""┤"",""┼"",""│"",""─"",""┐"",""└"",""┌""],[""┘"",""┬"",""├"",""┴"",""┤"",""┼"",""│""]],""treasures"":[[[""sphalerite"",""tigers-eye""],[""sphalerite"",""tanzanite-trillion""],[""sphalerite"",""super-seven""],[""purple-cabochon"",""tanzanite-trillion""],[""purple-cabochon"",""star-cabochon""],[""purple-cabochon"",""spinel""],[""purple-cabochon"",""sphalerite""]],[[""sphalerite"",""sunstone""],[""sphalerite"",""stilbite""],[""sphalerite"",""star-cabochon""],[""purple-cabochon"",""super-seven""],[""purple-cabochon"",""ruby""],[""purple-cabochon"",""ruby-diamond-profile""],[""purple-cabochon"",""rose-quartz""]],[[""sphalerite"",""spinel""],[""sphalerite"",""sphalerite""],[""ruby"",""zoisite""],[""purple-cabochon"",""sunstone""],[""purple-cabochon"",""rock-quartz""],[""purple-cabochon"",""rhodonite""],[""purple-cabochon"",""red-spinel-square-emerald-cut""]],[[""purple-cabochon"",""tourmaline""],[""purple-cabochon"",""tourmaline-laser-cut""],[""purple-cabochon"",""tigers-eye""],[""purple-cabochon"",""stilbite""],[""purple-cabochon"",""red-diamond""],[""purple-cabochon"",""raw-citrine""],[""purple-cabochon"",""raw-beryl""]],[[""purple-cabochon"",""purple-square-cushion""],[""purple-cabochon"",""purple-spinel-trillion""],[""purple-cabochon"",""purple-oval""],[""purple-cabochon"",""purple-cabochon""],[""prehnite"",""zoisite""],[""prehnite"",""zircon""],[""prehnite"",""yellow-jasper""]],[[""prehnite"",""yellow-heart""],[""prehnite"",""yellow-beryl-oval""],[""prehnite"",""yellow-baguette""],[""prehnite"",""white-square""],[""prehnite"",""unakite""],[""prehnite"",""tourmaline""],[""prehnite"",""tourmaline-laser-cut""]],[[""prehnite"",""tigers-eye""],[""prehnite"",""tanzanite-trillion""],[""prehnite"",""super-seven""],[""prehnite"",""sunstone""],[""prehnite"",""stilbite""],[""prehnite"",""star-cabochon""],[""prehnite"",""spinel""]]]},""last"":null,""plmt"":[{""color"":""orange"",""current"":{""column#"":2,""row#"":3},""goto"":{""column#"":1,""row#"":3},""home"":{""column#"":1,""row#"":3}},{""color"":""FFFFCC"",""current"":{""column#"":3,""row#"":1},""goto"":{""column#"":5,""row#"":5},""home"":{""column#"":1,""row#"":5}},{""color"":""blue"",""current"":{""column#"":1,""row#"":0},""goto"":{""column#"":3,""row#"":3},""home"":{""column#"":1,""row#"":1}},{""color"":""red"",""current"":{""column#"":2,""row#"":0},""goto"":{""column#"":3,""row#"":1},""home"":{""column#"":5,""row#"":3}},{""color"":""green"",""current"":{""column#"":3,""row#"":0},""goto"":{""column#"":1,""row#"":5},""home"":{""column#"":3,""row#"":5}}],""spare"":{""1-image"":""prehnite"",""2-image"":""ruby"",""tilekey"":""┤""}}";
  }
}