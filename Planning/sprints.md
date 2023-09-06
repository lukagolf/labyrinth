**TO:** Dot Game Company CEOs  
**FROM:** Can Ivit and Luka Jovanovic  
**DATE:** September 29, 2022  
**SUBJECT:** Plan for the First Three Sprints

**Note:**
We decided that designing anything network and GUI related in the first 3 sprints of the project would be an unrealistic plan. Our priority is to come up with a well designed and robust game that is playable locally with our own player implementations.

**Sprint 1:**  
- Come up with individual interfaces for all components described in [the components section](https://course.ccs.neu.edu/cs4500f22/components.html), and figure out how those interfaces will interact with each other with their public methods.
- Design any additional data definitions that will be used in the signatures of the main component interfaces as needed. 
- The signature, purpose statements, and any potential exceptions of the methods should be well documented.
- Come up with unit tests showing how individual interfaces must behave regardless of the underlying implementation. It is okay to write empty implementations so that the code compiles.

**Sprint 2:**
- Implement all the interfaces designed in Sprint 1 except the *Observers* and *Players*. 
- During the implementation, it is possible to discover minor flaws in the original interface design. Introduce the required changes as long as the documentation and the related tests are updated.
- Make sure all the implementations pass the tests written in Sprint 1.

**Sprint 3:**
- Write an implementation for *Observers*.
- Write at least two different *Player* implementations with different strategies. The strategies must be predictable so that they can be used for testing. 
- Write integration tests for the entire game using the *Observer* and *Player* implementations.
- Make sure integration tests cover all the edge cases like different game endings and players making invalid moves.    
