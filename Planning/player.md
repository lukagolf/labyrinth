**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** October 12, 2022  
**SUBJECT:** Design for the External Player

The Spec asks us to describe the data that a player must know to act. In milestone 3, we designed a part of this data definition when we were designing the referee's game state: [IGameState](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/main/Maze/Common/IGameState.cs). In addition to this data, a player must know its assigned treature to act. That's why we decided to create an interface that extends the IGameState.

```csharp
interface IPlayerGameState : IGameState
{
  // Gets the assigned treasure of the player. When a IExternalPlayer calls this property, they will get their assigned treasure.
  ITreasure AssignedTreasure { get; }
}

interface IExternalPlayer
{
  /* 
  Pushes the new game state information to the player. 
  Player can use the observation methods in IPlayerGameState to derive its strategy. Referee should call this method on each player after every move with the updated game state.
  */
  void NotifyGameState(IPlayerGameState gameState);

  // Retrieves the move this player wants to make.
  IMove RequestMove();
}
```
