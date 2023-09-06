using System;
using System.Collections.Generic;

namespace Common
{
  public static class MatrixExtensions
  {
    public static IEnumerable<(int rowIdx, int colIdx, T item)> Enumerate<T>(this T[,] matrix)
    {
      for (int rowIdx = 0; rowIdx < matrix.GetLength(0); rowIdx += 1)
      {
        for (int colIdx = 0; colIdx < matrix.GetLength(1); colIdx += 1)
        {
          yield return (rowIdx, colIdx, matrix[rowIdx, colIdx]);
        }
      }
    }

    public static void Fill<T>(this T[,] matrix, Func<int, int, T> func)
    {
      for (int rowIdx = 0; rowIdx < matrix.GetLength(0); rowIdx += 1)
      {
        for (int coldIdx = 0; coldIdx < matrix.GetLength(1); coldIdx += 1)
        {
          matrix[rowIdx, coldIdx] = func(rowIdx, coldIdx);
        }
      }
    }

    public static void ForEach<T>(this T[,] matrix, Action<int, int, T> func)
    {
      for (int rowIdx = 0; rowIdx < matrix.GetLength(0); rowIdx += 1)
      {
        for (int coldIdx = 0; coldIdx < matrix.GetLength(1); coldIdx += 1)
        {
          func(rowIdx, coldIdx, matrix[rowIdx, coldIdx]);
        }
      }
    }
  }
}