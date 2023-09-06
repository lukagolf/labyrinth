using System;
using Common;
using Xunit;

namespace UnitTests.Common
{
  public class RandomRefereeStateBuilderTests
  {
    private readonly IRefereeStateBuilder _stateBuilder;

    public RandomRefereeStateBuilderTests()
    {
      _stateBuilder = new RandomRefereeStateBuilder(new Random(38));
    }

    [Fact]
    public void TestCorrectBoardSize()
    {
      IRefereeState state = _stateBuilder.BuildState(2);
      Assert.Equal(5, state.Board.GetHeight());
      Assert.Equal(5, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(3);
      Assert.Equal(5, state.Board.GetHeight());
      Assert.Equal(5, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(4);
      Assert.Equal(5, state.Board.GetHeight());
      Assert.Equal(5, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(5);
      Assert.Equal(7, state.Board.GetHeight());
      Assert.Equal(7, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(6);
      Assert.Equal(7, state.Board.GetHeight());
      Assert.Equal(7, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(7);
      Assert.Equal(7, state.Board.GetHeight());
      Assert.Equal(7, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(8);
      Assert.Equal(7, state.Board.GetHeight());
      Assert.Equal(7, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(9);
      Assert.Equal(7, state.Board.GetHeight());
      Assert.Equal(7, state.Board.GetWidth());
      
      state = _stateBuilder.BuildState(10);
      Assert.Equal(9, state.Board.GetHeight());
      Assert.Equal(9, state.Board.GetWidth());
    }
  }
}