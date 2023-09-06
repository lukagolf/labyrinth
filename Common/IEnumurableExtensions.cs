using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;

namespace Common
{
  public static class EnumerableExtensions
  {
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> func)
    {
      foreach (var item in enumerable)
      {
        func(item);
      }
    }
  }
}