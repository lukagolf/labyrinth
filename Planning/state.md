**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** October 7, 2022  
**SUBJECT:** Design for the Game State

```csharp
interface IGameState 
{
  // checks if the player who has the current turn can do the given move
  bool IsValidMove(IMove move);

  // executes the move on behalf of the player who has the current turn
  void ExecuteMove(IMove move);

  // gets the unique id of the player who has the current turn
  string GetCurrentTurn();

  // gets the position of the given playerId
  BoardPosition GetPositionOfPlayer(string playerId);

  // gets the identifiers of all the players who are currently playing in their turn order
  IEnumerable<string> GetPlayers();

  // kicks the player out of the game
  void KickPlayerOut(string playerId);

  // did the given player collect their assigned treasure
  bool IsTresureCollected(string playerId);

  // is the game over?
  bool IsGameOver();

  // returns the identifier of the winning player if there is one
  Option<string> GetWinner();

  // returns the current spare tile
  ITile GetSpareTile();

  // returns the height of the board
  int GetBoardHeight();

  // returns the width of the board
  int GetBoardWidth();

  // returns the tile to the given position
  ITile GetTileAt(BoardPosition position);

  // returns the given player's home position
  BoardPosition GetHomePositionOfPlayer(string playerId);

  // returns the assigned treasure of the given player
  ITreasure GetTreasureOfPlayer(string playerId);
}

interface IMove
{
  ISlideAction Slide { get; }
  TileRotation Rotation { get; },
  BoardPosition Destination { get; }
}

interface ISlideAction
{
  SlideType Type { get; }
  int Index { get; }
}

enum TileRotation
{
  Ninty,
  OneHundredEighty,
  TwoHundredSeventy,
}

enum SlideType
{
  SlideRowLeft,
  SlideRowRight,
  SlideColumnUp,
  SlideColumnDown
}
```
