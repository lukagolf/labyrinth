using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using JsonUtilities;
using LanguageExt;
using Newtonsoft.Json;
using Players;
using Referee;
using Remote;

namespace Server
{
  public sealed class Server : IServer
  {
    private readonly int _signUpTimeout;
    private readonly int _nameTimeout;
    private readonly int _maxSignUpPeriods;
    private readonly int _minPlayers;
    private readonly int _maxPlayers;
    private readonly IReferee _referee;

    /// <summary>
    /// Constructs a TCP server where timeout and player capacity can be customized
    /// </summary>
    /// <param name="referee">The referee implementation to use when running games</param>
    /// <param name="signUpTimeout">Max number of seconds for the total signup period</param>
    /// <param name="nameTimeout">Max number of seconds remote players have to sign up their names</param>
    /// <param name="maxSignUpPeriods">How many times to repeat the sign up period before giving up</param>
    /// <param name="minPlayers">Min number of players to start the game with</param>
    /// <param name="maxPlayers">Max number of players to start the game with</param>
    public Server(IReferee referee, int signUpTimeout = 20, int nameTimeout = 2, int maxSignUpPeriods = 2,
      int minPlayers = 2, int maxPlayers = 6)
    {
      _signUpTimeout = ValidateSignUpTimeout(signUpTimeout);
      _nameTimeout = nameTimeout;
      _maxSignUpPeriods = ValidateMaxSignUpPeriods(maxSignUpPeriods);
      _minPlayers = ValidateMinPlayers(minPlayers);
      _maxPlayers = ValidateMaxPlayers(minPlayers, maxPlayers);
      _referee = referee;
    }

    public async Task<(IList<string> winningPlayers, IList<string> badPlayers)> RunAsync(IPAddress address, int port)
    {
      return await RunInternalAsync(address, port, Option<IRefereeState>.None);
    }

    public async Task<(IList<string> winningPlayers, IList<string> badPlayers)> RunAsync(IPAddress address, int port,
      IRefereeState state)
    {
      return await RunInternalAsync(address, port, Option<IRefereeState>.Some(state));
    }

    private async Task<(IList<string> winningPlayers, IList<string> badPlayers)> RunInternalAsync(IPAddress address,
      int port, Option<IRefereeState> maybeState)
    {
      var server = new TcpListener(address, port);
      server.Start();
      IList<ClientContact> clients = await SignUpClients(server);

      var result = clients.Count >= _minPlayers
        ? RunReferee(clients.Reverse(), maybeState)
        : (Array.Empty<string>(), Array.Empty<string>());

      EndConnections(clients);
      server.Stop();
      return result;
    }

    private async Task<IList<ClientContact>> SignUpClients(TcpListener server)
    {
      IList<ClientContact> clients = new List<ClientContact>();
      for (int i = 0; i < _maxSignUpPeriods; i += 1)
      {
        await CompleteOneSignUpPeriod(server, clients);
        if (clients.Count >= _minPlayers)
        {
          return clients;
        }
      }

      return clients;
    }

    private static void EndConnections(IEnumerable<ClientContact> contacts)
    {
      contacts.ForEach(contact => { contact.Client.Close(); });
    }

    private (IList<string> winningPlayers, IList<string> badPlayers) RunReferee(IEnumerable<ClientContact> clients,
      Option<IRefereeState> maybeState)
    {
      IList<IPlayer> players = clients.Select(CreateProxyPlayer).ToList();
      var result = maybeState.Match(
        state => _referee.RunGame(state, players),
        () => _referee.RunGame(players)
      );

      return (
        result.winningPlayers.Select(p => p.Name).ToList(),
        result.misbehavedPlayers.Select(p => p.Name).ToList()
      );
    }

    private static IPlayer CreateProxyPlayer(ClientContact clientContact)
    {
      return new ProxyPlayer(clientContact.Name, clientContact.Reader, clientContact.Writer);
    }

    private async Task CompleteOneSignUpPeriod(TcpListener server, IList<ClientContact> clients)
    {
      async Task ClientConnectionTask()
      {
        while (clients.Count < _maxPlayers)
        {
          await WaitSingleClientToConnect(server).IfSomeAsync(clients.Add);
        }
      }

      var delayTask = Task.Delay(TimeSpan.FromSeconds(_signUpTimeout));
      await Task.WhenAny(delayTask, ClientConnectionTask());
    }

    private async Task<Option<ClientContact>> WaitSingleClientToConnect(TcpListener server)
    {
      try
      {
        TcpClient client = await server.AcceptTcpClientAsync();
        var writer = new StreamWriter(client.GetStream(), new UTF8Encoding(false));
        var reader = new StreamReader(client.GetStream(), new UTF8Encoding(false));
        var jsonReader = new JsonTextReader(reader);
        var delayTask = Task.Delay(TimeSpan.FromSeconds(_nameTimeout));
        var completed = await Task.WhenAny(delayTask, jsonReader.ReadAsync());
        if (completed == delayTask)
        {
          return Option<ClientContact>.None;
        }

        string name = CustomSerializer.Instance.Deserialize<string>(jsonReader)!;
        return new ClientContact(name, client, reader, writer);
      }
      catch (Exception)
      {
        return Option<ClientContact>.None;
      }
    }

    private static int ValidateSignUpTimeout(int signUpTimeout)
    {
      if (signUpTimeout < 1)
      {
        throw new ArgumentException("Sign up period cannot be less than 1 second.");
      }

      return signUpTimeout;
    }

    private static int ValidateMaxSignUpPeriods(int maxSignUpPeriods)
    {
      if (maxSignUpPeriods < 1)
      {
        throw new ArgumentException("There must be at least 1 sign up period.");
      }

      return maxSignUpPeriods;
    }

    private static int ValidateMinPlayers(int minPlayers)
    {
      if (minPlayers < 1)
      {
        throw new ArgumentException("There must be at least 1 player sign up.");
      }

      return minPlayers;
    }

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private static int ValidateMaxPlayers(int minPlayers, int maxPlayers)
    {
      if (maxPlayers < minPlayers)
      {
        throw new ArgumentException("Maximum number of players cannot be less than minimum number of players.");
      }

      return maxPlayers;
    }

    private sealed class ClientContact
    {
      public ClientContact(string name, TcpClient client, TextReader reader, TextWriter writer)
      {
        Name = name;
        Client = client;
        Reader = reader;
        Writer = writer;
      }

      public string Name { get; }
      public TcpClient Client { get; }
      public TextReader Reader { get; }
      public TextWriter Writer { get; }
    }
  }
}