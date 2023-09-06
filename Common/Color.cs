using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Common
{
  public class Color : IEquatable<Color>
  {
    public static readonly Color Purple = new(255, 0, 255);
    public static readonly Color Orange = new(255, 165, 0);
    public static readonly Color Pink = new(255, 105, 180);
    public static readonly Color Red = new(255, 0, 0);
    public static readonly Color Blue = new(0, 0, 255);
    public static readonly Color Green = new(0, 255, 0);
    public static readonly Color Yellow = new(255, 255, 0);
    public static readonly Color White = new(255, 255, 255);
    public static readonly Color Black = new(0, 0, 0);
    
    private static readonly IDictionary<string, Color> ValidColorNames = new Dictionary<string, Color>
    {
      ["purple"] = Purple,
      ["orange"] = Orange,
      ["pink"] = Pink,
      ["red"] = Red,
      ["blue"] = Blue,
      ["green"] = Green,
      ["yellow"] = Yellow,
      ["white"] = White,
      ["black"] = Black,
    };

    private static readonly Regex HexColorExp = new(@"^[A-F|\d][A-F|\d][A-F|\d][A-F|\d][A-F|\d][A-F|\d]$");

    public Color(byte redValue, byte greenValue, byte blueValue)
    {
      RedValue = redValue;
      GreenValue = greenValue;
      BlueValue = blueValue;
    }

    public static Color Create(string nameOrHex)
    {
      return ValidColorNames.ContainsKey(nameOrHex) ? ValidColorNames[nameOrHex] : CreateFromHex(nameOrHex);
    }

    public byte RedValue { get; }
    public byte GreenValue { get; }
    public byte BlueValue { get; }

    private static Color CreateFromHex(string hexCode)
    {
      if (!HexColorExp.IsMatch(hexCode))
      {
        throw new ArgumentException("Invalid hex color");
      }

      byte red = byte.Parse(hexCode.Substring(0, 2), NumberStyles.HexNumber);
      byte green = byte.Parse(hexCode.Substring(2, 2), NumberStyles.HexNumber);
      byte blue = byte.Parse(hexCode.Substring(4, 2), NumberStyles.HexNumber);

      return new Color(red, green, blue);
    }

    public bool Equals(Color? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return RedValue == other.RedValue && GreenValue == other.GreenValue && BlueValue == other.BlueValue;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(RedValue, GreenValue, BlueValue);
    }

    public override string ToString()
    {
      return Convert.ToHexString(new[] {RedValue, GreenValue, BlueValue});
    }
  }
}