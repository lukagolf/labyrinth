**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic   
**DATE:** November 3, 2022  
**SUBJECT:** Remote Games

## Gathering Players:
There is going to be TCP server running at a known address and a port number. Players who would like to participate in a game running on that server need to wire up their PlayerApi implementations to the following wrapper
```csharp
public class TcpPlayer 
{
  // A PlayerApi implementation that follows the protocol defined in the Logical Interactions
  private readonly IPlayer _player;
  private readonly TcpClient _tcpClient;

  // Constructs this wrapper class around a player api. The wrapper uses a tcp socket to comminicate with the server using a json protocol and delegates to the given player api implementation
  TcpPlayer(IPlayer player)
  {
    _player = player;
    _tcpClient = new TcpClient();
  }

  // Tries to join a game running on a TCP server
  // Returns one of:
  // - Success: If player successfully joins and finishes a game
  // - Fail: If the connection to the server fails
  public Either<Success, Fail> Play(string serverAddress, int serverPort);
}
``` 

## Launching a Game:
Once a TCP server has n connected clients (n is a constant positive integer that can depend on the server implementation), it will wrap the client sockets around proxy player classes that implement the IPlayer so that the referee can talk to them directly. Then, the server will create a referee and start the game by passing the players to the referee.
```csharp
public class ProxyPlayer : IPlayer
{
  private readonly TcpClient _tcpClient;

  // ProxyPlayer uses the given tcpClient to implement the IPlayer methods. 
  // It will pass the referee's requests to the real player over the network through the tcp client using a json protocol
  ProxyPlayer(TcpClient tcpClient)
  {
    _tcpClient = tcpClient;
  }

  public Task<string> Name { get; }
  public Task<IBoard> ProposeBoard(int rows, int columns);
  public Task<Acknowledge> Setup(Option<IPlayerState> initialState, BoardPosition goal);
  public Task<Either<IMove, Pass>> TakeTurn(IPlayerState state);
  public Task<Acknowledge> Won(bool won);
}

```