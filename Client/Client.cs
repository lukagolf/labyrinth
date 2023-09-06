using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JsonUtilities;
using Players;
using Remote;

namespace Client
{
  public class Client : IClient
  {
    private readonly IPlayerDispatcher _playerDispatcher;
    private readonly int _reconnectInterval;

    /// <param name="playerDispatcher">The player-dispatcher implementation to use to get the result</param>
    /// <param name="reconnectInterval">
    /// Number of milliseconds to wait after each failed connection attempt to the server
    /// </param>
    public Client(IPlayerDispatcher playerDispatcher, int reconnectInterval)
    {
      _playerDispatcher = playerDispatcher;
      _reconnectInterval = reconnectInterval;
    }

    public async Task<Result> RunAsync(IPAddress server, int port, IPlayer player)
    {
      PlayerContact contact = await SignUpPlayer(server, port, player);
      var result = await _playerDispatcher.RunAsync(contact.Reader, contact.Writer, contact.Player);
      contact.Client.Close();
      return result;
    }

    private async Task<PlayerContact> SignUpPlayer(IPAddress server, int port, IPlayer player)
    {
      TcpClient client = await ConnectToServer(server, port);
      TextWriter writer = new StreamWriter(client.GetStream(), new UTF8Encoding(false));
      TextReader reader = new StreamReader(client.GetStream(), new UTF8Encoding(false));
      CustomSerializer.Instance.Serialize(writer, player.Name);
      await writer.FlushAsync();
      return new PlayerContact(player, client, reader, writer);
    }

    private async Task<TcpClient> ConnectToServer(IPAddress server, int port)
    {
      while (true)
      {
        try
        {
          var client = new TcpClient();
          await client.ConnectAsync(server, port);
          return client;
        }
        catch (Exception)
        {
          await Task.Delay(TimeSpan.FromMilliseconds(_reconnectInterval));
        }
      }
    }

    private sealed class PlayerContact
    {
      public PlayerContact(IPlayer player, TcpClient client, TextReader reader, TextWriter writer)
      {
        Player = player;
        Client = client;
        Reader = reader;
        Writer = writer;
      }

      public IPlayer Player { get; }
      public TcpClient Client { get; }
      public TextReader Reader { get; }
      public TextWriter Writer { get; }
    }
  }
}