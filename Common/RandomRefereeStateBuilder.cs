using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
  public class RandomRefereeStateBuilder : IRefereeStateBuilder
  {
    private readonly Random _random;

    public RandomRefereeStateBuilder(Random random)
    {
      _random = random;
    }

    public RandomRefereeStateBuilder() : this(new Random())
    {
    }

    public IRefereeState BuildState(int numberOfPlayers)
    {
      int boardLength = FindMinBoardLength(numberOfPlayers);
      return BuildState(boardLength, boardLength, numberOfPlayers);
    }

    public IRefereeState BuildState(int boardHeight, int boardWidth, int numberOfPlayers)
    {
      IList<ITreasure> treasures = CreateDistinctTreasures(boardHeight * boardWidth + 1);
      IBoard board = CreateBoard(boardHeight, boardWidth, treasures);
      if (numberOfPlayers > NumberOfDistinctHomes(board))
      {
        throw new ArgumentException("Board is too small for the number of players");
      }

      ITile spareTile = CreateRandomTile(treasures[treasures.Count - 1]);
      IList<IPlayerInfo> players = CreateInternalPlayers(board, numberOfPlayers);
      return new RefereeState(players, board, spareTile);
    }

    private static int FindMinBoardLength(int numberOfPlayers)
    {
      return ((int) Math.Ceiling(Math.Sqrt(numberOfPlayers))) * 2 + 1;
    }

    private static int NumberOfDistinctHomes(IBoard board)
    {
      int immovableRows = board.GetHeight() - board.SlidableRows.Count();
      int immovableColumns = board.GetWidth() - board.SlidableColumns.Count();
      return immovableRows * immovableColumns;
    }

    private IList<IPlayerInfo> CreateInternalPlayers(IBoard board, int numberOfPLayers)
    {
      IList<Color> playerColors = CreateDistinctColors(numberOfPLayers);
      var immovablePositions = ImmovablePositions(board).ToList();
      IList<BoardPosition> homePositions = CreateHomePositions(numberOfPLayers, immovablePositions);
      IList<BoardPosition> currentPositions =
        CreateCurrentPositions(numberOfPLayers, board.GetHeight(), board.GetWidth());
      IList<BoardPosition> goalPositions = CreateGoalPositions(numberOfPLayers, immovablePositions);

      return Enumerable.Range(0, numberOfPLayers).Select(i =>
        {
          Color color = playerColors[i];
          BoardPosition home = homePositions[i];
          BoardPosition current = currentPositions[i];
          ITreasure treasure = board.GetTileAt(goalPositions[i]).Treasure;
          return (IPlayerInfo) new PlayerInfo(color, home, current, treasure);
        })
        .ToList();
    }

    private static IEnumerable<BoardPosition> ImmovablePositions(IBoard board)
    {
      ISet<int> movableRows = board.SlidableRows.ToHashSet();
      ISet<int> movableColumns = board.SlidableColumns.ToHashSet();
      
      IList<int> immovableRows = Enumerable.Range(0, board.GetHeight())
        .Where(r => !movableRows.Contains(r))
        .ToList();
      
      IList<int> immovableColumns = Enumerable.Range(0, board.GetWidth())
        .Where(c => !movableColumns.Contains(c))
        .ToList();
      
      foreach (int rowIdx in immovableRows)
      {
        foreach (int colIdx in immovableColumns)
        {
          yield return new BoardPosition(rowIdx, colIdx);
        }
      }
    }

    private IList<Color> CreateDistinctColors(int numberOfPlayers)
    {
      ISet<Color> colors = new HashSet<Color>();
      while (colors.Count < numberOfPlayers)
      {
        byte red = (byte) _random.Next(0, 255);
        byte green = (byte) _random.Next(0, 255);
        byte blue = (byte) _random.Next(0, 255);
        colors.Add(new Color(red, green, blue));
      }

      return colors.ToList();
    }

    private IBoard CreateBoard(int height, int width, IList<ITreasure> treasures)
    {
      var tileMatrix = new ITile[height, width];
      tileMatrix.Fill((rowIdx, colIdx) => CreateRandomTile(treasures[(rowIdx * width) + colIdx]));
      return new Board(tileMatrix);
    }

    private ITile CreateRandomTile(ITreasure treasure)
    {
      var tiles = new List<ITile>
      {
        new Tile(true, false, true, false, treasure),
        new Tile(true, false, false, true, treasure),
        new Tile(false, true, true, true, treasure),
        new Tile(true, true, true, true, treasure),
      };

      IList<Rotation> rotations = Enum.GetValues<Rotation>().ToList();
      ITile tile = tiles[_random.Next(tiles.Count)];
      Rotation rotation = rotations[_random.Next(rotations.Count)];
      return tile.Rotate(rotation);
    }

    private IList<ITreasure> CreateDistinctTreasures(int numberOfTreasures)
    {
      IList<Gem> gems = Enum.GetValues<Gem>().ToList();
      ISet<ITreasure> treasures = new HashSet<ITreasure>();
      while (treasures.Count < numberOfTreasures)
      {
        Gem gem1 = gems[_random.Next(gems.Count)];
        Gem gem2 = gems[_random.Next(gems.Count)];
        ITreasure treasure = new Treasure(gem1, gem2);
        treasures.Add(treasure);
      }

      return treasures.ToList();
    }

    private IList<BoardPosition> CreateHomePositions(int numberOfPlayers, IList<BoardPosition> possibleHomes)
    {
      ISet<BoardPosition> set = new HashSet<BoardPosition>();
      while (set.Count < numberOfPlayers)
      {
        set.Add(possibleHomes[_random.Next(possibleHomes.Count)]);
      }

      return set.ToList();
    }

    private IList<BoardPosition> CreateCurrentPositions(int numberOfPlayers, int boardHeight, int boardWidth)
    {
      return Enumerable.Range(0, numberOfPlayers)
        .Select(_ => new BoardPosition(_random.Next(boardHeight), _random.Next(boardWidth)))
        .ToList();
    }

    private IList<BoardPosition> CreateGoalPositions(int numberOfPlayers, IList<BoardPosition> possibleHomes)
    {
      return Enumerable.Range(0, numberOfPlayers)
        .Select(_ => possibleHomes[_random.Next(possibleHomes.Count)])
        .ToList();
    }
  }
}