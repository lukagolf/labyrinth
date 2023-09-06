using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Client;
using Common;
using Players;
using Referee;
using Remote;
using Server;
using UnitTests.RefereeTests;
using Xunit;
using static UnitTests.RefereeTests.RefereeTestUtilities;

namespace UnitTests.Remote
{
  public class ServerAndClientTests
  {
    /// <summary>
    /// Test when a single player successfully reaches home after capturing their assigned treasure
    /// </summary>
    [Fact]
    public async Task Test1()
    {
      var localPurplePlayer = new MockPlayer("purple", new PassStrategy());
      var remotePurplePlayer = new MockPlayer("purple", new PassStrategy());

      var localOrangePlayer = new MockPlayer("orange", new PassStrategy());
      var remoteOrangePlayer = new MockPlayer("orange", new PassStrategy());

      var localPinkPlayer = new MockPlayer("pink", new EuclidStrategy());
      var remotePinkPlayer = new MockPlayer("pink", new EuclidStrategy());

      var localPlayers = new List<IPlayer> {localPurplePlayer, localOrangePlayer, localPinkPlayer};
      var remotePlayers = new List<IPlayer> {remotePurplePlayer, remoteOrangePlayer, remotePinkPlayer};
      IRefereeState localState = CreateGameStateWinByReachingHome();
      IRefereeState remoteState = CreateGameStateWinByReachingHome();

      await TestSuccessfulSignupAndGamePlay(localPlayers, remotePlayers, localState, remoteState);
    }

    /// <summary>
    /// Test that the referee kicks out players who throw exception in the take turn stage
    /// </summary>
    [Fact]
    public async Task Test2()
    {
      var localPurplePlayer = new MockPlayer("purple", new PassStrategy());
      var remotePurplePlayer = new MockPlayer("purple", new PassStrategy());

      var localOrangePlayer = new MockPlayer("orange", new ThrowExceptionStrategy(5));
      var remoteOrangePlayer = new MockPlayer("orange", new ThrowExceptionStrategy(5));

      var localPinkPlayer = new MockPlayer("pink", new EuclidStrategy());
      var remotePinkPlayer = new MockPlayer("pink", new EuclidStrategy());

      var localPlayers = new List<IPlayer> {localPurplePlayer, localOrangePlayer, localPinkPlayer};
      var remotePlayers = new List<IPlayer> {remotePurplePlayer, remoteOrangePlayer, remotePinkPlayer};
      IRefereeState localState = CreateGameStateWinByReachingHome();
      IRefereeState remoteState = CreateGameStateWinByReachingHome();

      await TestSuccessfulSignupAndGamePlay(localPlayers, remotePlayers, localState, remoteState);
    }

    /// <summary>
    /// Test that the referee kicks out players who take too long in win request
    /// </summary>
    [Fact]
    public async Task Test3()
    {
      var localPurplePlayer = new MockPlayer("purple", new PassStrategy());
      var remotePurplePlayer = new MockPlayer("purple", new PassStrategy());

      var localOrangePlayer = new MockPlayer("orange", new PassStrategy()) {TakeTooLongInWon = true,};
      var remoteOrangePlayer = new MockPlayer("orange", new PassStrategy()) {TakeTooLongInWon = true,};

      var localPinkPlayer = new MockPlayer("pink", new EuclidStrategy()) {TakeTooLongInWon = true,};
      var remotePinkPlayer = new MockPlayer("pink", new EuclidStrategy()) {TakeTooLongInWon = true,};

      var localPlayers = new List<IPlayer> {localPurplePlayer, localOrangePlayer, localPinkPlayer};
      var remotePlayers = new List<IPlayer> {remotePurplePlayer, remoteOrangePlayer, remotePinkPlayer};
      IRefereeState localState = CreateGameStateWinByReachingHome();
      IRefereeState remoteState = CreateGameStateWinByReachingHome();

      await TestSuccessfulSignupAndGamePlay(localPlayers, remotePlayers, localState, remoteState);
    }

    private static async Task TestSuccessfulSignupAndGamePlay(IList<IPlayer> localPlayers,
      IList<IPlayer> remotePlayers, IRefereeState localState, IRefereeState remoteState)
    {
      var localResult = RunLocalGame(localPlayers, localState);
      var remoteResult = await RunRemoteGame(remotePlayers, remoteState);
      Assert.Equal(localResult.winners, remoteResult.winners);
      Assert.Equal(localResult.badPlayers, remoteResult.badPlayers);
    }

    private static (IList<string> winners, IList<string> badPlayers) RunLocalGame(IList<IPlayer> players,
      IRefereeState state)
    {
      IReferee referee = new Referee.Referee(new RuleBook(), 1000);
      var result = referee.RunGame(state, players);
      return (
        result.winningPlayers.Select(w => w.Name).ToList(),
        result.misbehavedPlayers.Select(w => w.Name).ToList()
      );
    }

    private static async Task<(IList<string> winners, IList<string> badPlayers)> RunRemoteGame(IList<IPlayer> players,
      IRefereeState state)
    {
      IReferee referee = new Referee.Referee(new RuleBook(), 500);
      IServer server = new Server.Server(referee, signUpTimeout: 10, maxSignUpPeriods: 1, nameTimeout: 2);
      var serverTask = server.RunAsync(IPAddress.Parse("127.0.0.1"), 60140, state);
      IPlayerDispatcher playerDispatcher = new PlayerDispatcher();
      IClient client = new Client.Client(playerDispatcher, 100);
      IMultiClient multiClient = new MultiClient(client, 1000);
      await multiClient.RunAsync(IPAddress.Parse("127.0.0.1"), 60140, players.Reverse().ToList());
      return await serverTask;
    }
  }
}