**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** November 26, 2022  
**SUBJECT:** The revisions to support multiple treasure chase 

# The Revisions

1. We did not make any changes to the PlayerAPI.

2. The referee holds an IReferee state which represents the ground truth of the game. IReferee state has a collection of IPlayerInfo, which are the internal representations of the players in the state. To support multiple treasure chase, we replaced some of the properties in IPlayerInfo with new ones.

    - [Commit 1](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/66074da222ed1b30657bfcd106b55b3e0baa3345)

    - [Commit 2](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/d9bf341d96274643af8b78a90e48f5dda8ad42ef)

    - [Commit 3](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/343062e14e97dc66f0be959eb5fddf3dc30d62dc)

    - [Commit 4](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/9564e843764464ee558df3dcdd2083db3a62dfb6)

    - [Commit 5](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/c6a213fb682f2e5269bfee0e3eb0e22fb0bc9291)

    Explanation for commits above:  
    
    - We renamed AssignedTreasure to CurrentlyAssignedTreasure because a player can have multiple assigned treasures throughout the game.
    - We added a new property called CapturedTreasures, which is a set of treasures the player has captured so far. This must be a set because capturing the same treasure multiple times should not be possible.
    - We added a new property called NumberOfNumberOfAssignedTreasures because when scoring the game, we need to decide whether we should calculate the euclidian distance to the home coordinate or the coordinate of the next treasure the player needs to capture. We do that by checking if the number of items in CapturedTreasures is equal to the NumberOfAssignedTreasures.
    - UPDATE: We realized that our current implementation did not produce the correct game result when a player's home and goal tile are on the same tile. Previously, the player won right after stepping on the goal, which is not correct. The player can only win on the next round after capturing the assigned treasure. To fix this bug, we first wrote a failing unit test, and then made our revisions in Commit 4. We added a new property to IPlayerInfo with the type Option(int) called "CapturedAllAssignedTreasuresRoundIdx". This property represents the round index of when the player has captured all of its assigned treasures, if they did. This allows the referee to check whether capturing the treasure and reaching home happens in different rounds. We confirmed that this fixes the bug because the failing unit test now passes. Also, addition of this new property made the NumberOfNumberOfAssignedTreasures property pointless, because the fact that whether player captured all of its assigned treasures can be checked with the new property. That's why we removed NumberOfNumberOfAssignedTreasures property.
    - Commit 5 fixes a typo in the purpose statement of "CapturedAllAssignedTreasuresRoundIdx" property. 
3. We made changes to the referee interface and implementation.
    - We added two new signatures to the referee interface that takes in the ordered sequence of potential goals.

      [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/6bfb7233028caf54721b1024f10e57ba045d29e6)  
      Explanation: The signature of our previous referee methods are not capable of taking in an ordered sequence of potential goals. 

    - Based on the new IPlayerInfo properties explained in item 2, we changed how the referee handles treasure captures, how it determines the game is over, and how it scores the end game.  
    [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/b0aed919c5f05321d0d4f483e6a1b473ee973265)  
      Explanation: The new private helper methods we added have purpose statemens.  