using System;
using Common;
using Xunit;

namespace UnitTests.Common
{
  public class ColorTests
  {
    [Fact]
    public void TestValidHex()
    {
      var color = Color.Create("608A67");
      Assert.Equal(96, color.RedValue);
      Assert.Equal(138, color.GreenValue);
      Assert.Equal(103, color.BlueValue);

      color = Color.Create("6E9CC2");
      Assert.Equal(110, color.RedValue);
      Assert.Equal(156, color.GreenValue);
      Assert.Equal(194, color.BlueValue);
    }
    
    [Fact]
    public void TestInvalidHex()
    {
      Assert.Throws<ArgumentException>(() => Color.Create("608A6"));
      Assert.Throws<ArgumentException>(() => Color.Create("6e9cc2"));
    }
    
    [Fact]
    public void TestValidColorName()
    {
      var color = Color.Create("red");
      Assert.Equal(255, color.RedValue);
      Assert.Equal(0, color.GreenValue);
      Assert.Equal(0, color.BlueValue);
      
      color = Color.Create("green");
      Assert.Equal(0, color.RedValue);
      Assert.Equal(255, color.GreenValue);
      Assert.Equal(0, color.BlueValue);
      
      color = Color.Create("blue");
      Assert.Equal(0, color.RedValue);
      Assert.Equal(0, color.GreenValue);
      Assert.Equal(255, color.BlueValue);
      
      color = Color.Create("purple");
      Assert.Equal(255, color.RedValue);
      Assert.Equal(0, color.GreenValue);
      Assert.Equal(255, color.BlueValue);
    }

    [Fact]
    public void TestInvalidColorName()
    {
      Assert.Throws<ArgumentException>(() => Color.Create("cyan"));
      Assert.Throws<ArgumentException>(() => Color.Create("gray"));
      Assert.Throws<ArgumentException>(() => Color.Create("magenta"));
    }
  }
}