**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** November 8, 2022  
**SUBJECT:** Redesign difficulty rankings

- ## Blank tiles for the board: 1
This is a very easy change because our tile constructor takes in four boolean values indicating up, left, down, right connections. We just need to remove the validation that a tile must have at least two connections.   
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Common/Tile.cs#L26)

- ## Use movable tiles as goals: 1
There are no changes necessary because in the referee state players goals are stored as treasures, not board positions.   
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Common/IPlayerInfo.cs#L9)

- ## Ask player to sequentially pursue several goals, one at a time : 4
Significant changes are necessary because the representation of the player in the state only has one assigned treasure. We would need to changes this to be a list of assigned treasures. In addition, the referee implementation would need to change. The referee must check if a player reached their first treasure and if they did update the player's assigned treasure list in the state and send a setup request to the PlayerAPI with the new goal in the list.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Common/IPlayerInfo.cs#L9)