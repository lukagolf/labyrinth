**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** October 18, 2022  
**SUBJECT:** Player-Referee Protocol 

We have the following interface that outside players need to use to communicate with the referee. This document explains the calling sequence of these methods:

- The refereee holds an `IPlayer` reference for each player in the game as well as a reference to an `IState` that represents the ground truth of the game.

- When the game starts, and after each player performs a move, the referee will call `NotifyGameState` with the current state of the game. If they want, the players can use this state to derive their strategy and render the game visually.
  - `NotifyGameState` also serves the purpose of notifying players about who got kicked out because this information is accessebile through `IPlayerState` interface.

- The referee will ask the currently active player what is their desired move by calling `RequestMove` on that player. The player must respond within the given timeout of milliseconds, otherwise the referee will kick the player out. Players can respond with one of these two values:
  - `IMove`, if they want to perform a move
  - `None`, if they want to take a pass

  If the player's move is not valid according to the referee's rules, the referee will kick the player out.

- When the game is over, the referee will notify the players by calling `NotifyGameOver` method on each player with the public information of the winning player, if there is one.


```csharp
  /// <summary>
  /// Represents a player that referee will communicate with to get the player's desired moves and to push game
  /// state updates to the player 
  /// </summary>
  public interface IPlayer
  {
    /// <summary>
    /// Pushes the new game state information to the player.
    /// Player can use the observation methods in IPlayerState to derive its strategy.
    /// Referee should call this method on each player after every move with the updated game state.
    /// </summary>
    /// <param name="state">The updated game state</param>
    public void NotifyGameState(IPlayerState state);

    /// <summary>
    /// Returns a task (promise) that will be resolved to the move player wants to perform.
    /// </summary>
    /// <param name="timeout">
    /// The maximum time player can take to think before responding with their desired move in milliseconds.
    /// If the player fails to respond within this time interval, they will get kicked out.
    /// </param>
    /// <returns>
    /// A task of one of:
    /// - IMove: if the player wants to perform a move
    /// - None: if the player wants to take a pass
    /// </returns>
    public Task<Option<IMove>> RequestMove(int timeout);

    /// <summary>
    /// Notifies the player that the game is over and who won, if there is a winner.
    /// </summary>
    /// <param name="winner">
    /// One of:
    /// - IPublicPlayerInfo: Public information of the winning player if there is a winner
    /// - None: If there is no winner
    /// </param>
    public void NotifyGameOver(Option<IPublicPlayerInfo> winner);
  }
```
