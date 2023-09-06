**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** November 6, 2022  
**SUBJECT:** Cleanup Tasks

# Todo List In The Order of Priority

`Note:`  
We ran the instructor tests for all milestones from 3 to 5. They all pass, so we don't have any integration tests to worry about.



1. Right now, our referee implementation does not check for an acknowledgement from the players after calling the 'won' method. We should change this so that the referee puts the players who failed to acknowledge a call to 'won' in list of misbehaved players.
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Referee/Referee.cs#L108)

2. Our original state implementation included rules of the game such as "Players can't undo a previous slide" and "Players must move to a different tile after sliding a row or column". However, we later on discovered that this is a design flaw. The state of the game, the rules of the game, and the interpretation of the first according to the second are three distinct, disjoint, and unrelated concerns. That's why, in the middle of milestone 4, we decided to build a IRule interface that the referee can use to enforce game rules independent of the state. This forced us to comment out some of our state unit tests for because of time constraints. We will make sure we have unit tests for all state methods.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/UnitTests/Common/StateTests.cs#L9)

3. We have a single function that takes in a lambda (which will invoke a method on the player) and calls that lambda safely by timing it and handling any exceptions the players can throw. The referee uses this function by first creating a lazy lambda around a player method invoke and passing this lazy lambda to this function. However, the referee needs to remember to this every time it wants to invoke a method on the players and manually kick out the misbaheved players. If the referee forgets to kick out the misbehaved players, the end game result would be wrong. Worse, if the referee forgets to create a lazy lambda and directly calls a method on the player, the referee will crash if the player throws an exception. That's why we need to build a 'ProtectedPlayer' that wraps the players and provides the protection the referee needs (timeout and exception handling). The referee will directly call methods on these protected players without interaction with real players at all. The protected players should also remove the information of misbehaved players from the game state automatically.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Referee/Referee.cs#L320)

4. We protected the referee from the calls to the observers in the same way we did for the players as explained in item 3. We will improve this protection by creating a ProtectedObserver similar to the ProtectedPlayer explained in item 3.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Referee/Referee.cs#L149)

5. The GuiObserver should not depend on a specific IStateDrawer or IObserverModel implementation. Also, ObserverForm should not take in an IStateDrawer inside its constructor. Drawing the state is not the job of the form, it is job of the observer. The form should just display the given image.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Observer/GuiObserver.cs#L22)

6. We had 'IPlayerState' interface. This interface extends the IPublicState interface with additional methods to allow players see only their own assigned treasure and experiment with different row, column slides consecutively without mutating the actual state. Now that we see the player API, we understand that a state created specifically for one player is not needed anymore since the referee informs the players their goal tiles in addition to the public state in the setup method. Also, we rezlied that it is not the referee's job to provide utility functions to players to allow them to easily test out different slides/moves. The referee's job just informing the players about the public state of the game. That's why we think removing IPlayerState would improve the overall codebase my removing unncessary fluff.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Players/IPlayerState.cs#L10)

7. We currently have two interfaces for state: IPublicState and IState.
IPublicState represents only the public information of the game while the IState represents the referee's state that includes private player information. IState extends IPublicState. As for now, we only have an implementation for the IState and this powerful state gets downgraded to the IPublicState when it is passed into a method that accepts an IPublicState such as PlayerAPI methods. However, when we have remote players, we need to serialize IPublicState into JSON and parse it back into an IPublicState on the remote end. Since, with the current design, we first need to construct an IState to have an IPublicState we have to make up some information such as assigned player treasure. Obviously, this is not the best design so, we need an implementation for IPublicState. This implementation and our current IState implementation should not contain duplicate code, therefore we need to abstract.

8. In our strategies, we assumed all board implemenations will have even numbered row and column indices as slidable rows and columns. However, this may not always be the case. That's why we added two new methods into the board interface that can be used to check which rows and columns of that board are slidable.  
[Link](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/blob/before-todo/Maze/Players/GoalStrategy.cs#L23)

9. Impose the restriction that the players’ homes are distinct tiles.

# Completed

`Note:` The commits for item 1 and 3 are the same because we decided that solving them together would make sense since they are related.

1. [Commit1](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/8282745a3723709e253c392e8aa765d2455b11b6), [Commit2](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/109b47ff9f8c3e76a0fe4ba97dc4fa3055fcc022)

2. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/9b3e796bdc90af29f524629e48aa28db1a6255b1)

3. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/8282745a3723709e253c392e8aa765d2455b11b6)

4. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/f33765db69e77ecc85963d19b109e95be2d516c6)

5. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/c45e7878db28eb0d7bfb42f1eb2948127c21c830)

6. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/ecc9790497fcf1e4693151ca2dccd097198a2b1c)

7. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/4b3f7018530c78b0d7032638f4ec462a9777e96c)

8. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/6188ae05888dd685efad42950bfcceb2fe0e2194)

9. [Commit](https://github.khoury.northeastern.edu/CS4500-F22/canivit-jovanovicl/commit/bb76801fb9bb6c695983b4129de7758a861c0a83)