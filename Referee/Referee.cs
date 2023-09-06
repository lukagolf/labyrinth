using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using LanguageExt;
using Players;

namespace Referee
{
  /// <summary>
  /// Represents a referee that runs a maze game by
  /// - validating the player's move using the given rule
  /// - kicking out players who tries to make an invalid move and longer to respond than the given timeout
  /// - ending the game if the game does not end before the given maximum number of rounds
  /// </summary>
  public class Referee : IReferee
  {
    /// <summary>
    /// The game rules this referee will use to check the validity of players' moves
    /// </summary>
    private readonly IRule _rule;

    private readonly IRefereeStateBuilder _stateBuilder;

    /// <summary>
    /// The maximum number of milliseconds the player can take to respond to referee's requests
    /// </summary>
    private readonly int _responseTimeout;

    /// <summary>
    /// The referee will end the game if it does not end before this many rounds
    /// </summary>
    private readonly int _maxRounds;

    private Referee(IRule rule, IRefereeStateBuilder stateBuilder, int responseTimeout, int maxRounds = 1000)
    {
      _rule = rule;
      _stateBuilder = stateBuilder;
      _responseTimeout = responseTimeout;
      _maxRounds = maxRounds;
    }

    public Referee(IRule rule, int responseTimeout, int maxRounds = 1000) : this(rule, new RandomRefereeStateBuilder(),
      responseTimeout, maxRounds)
    {
    }

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IList<IPlayer> players)
    {
      return RunGame(players, new List<IObserver>());
    }

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IList<IPlayer> players,
      IList<IObserver> observers)
    {
      return RunGame(_stateBuilder.BuildState(players.Count), players, observers);
    }

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IRefereeState state,
      IList<IPlayer> players)
    {
      return RunGame(state, players, new List<IObserver>());
    }

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IRefereeState state,
      IList<IPlayer> players, IList<IObserver> observers)
    {
      return RunGame(state, new Queue<ITreasure>(), players, observers);
    }

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IRefereeState state,
      Queue<ITreasure> treasures, IList<IPlayer> players)
    {
      return RunGame(state, treasures, players, new List<IObserver>());
    }

    public (IList<IPlayer> winningPlayers, IList<IPlayer> misbehavedPlayers) RunGame(IRefereeState state,
      Queue<ITreasure> treasures, IList<IPlayer> players, IList<IObserver> observers)
    {
      IList<ProtectedPlayer> protectedPlayers = ProtectPlayers(players, state);
      IList<ProtectedObserver> protectedObservers = ProtectObservers(observers);
      var result = RunGameSafe(state, protectedPlayers, protectedObservers);
      var winningPlayers = result.winningPlayers.Select(w => w.GetPlayerUnsafe()).ToList();
      var misbehavedPlayers = result.misbehavedPlayers.Select(w => w.GetPlayerUnsafe()).ToList();
      return (winningPlayers, misbehavedPlayers);
    }

    private IList<ProtectedPlayer> ProtectPlayers(IList<IPlayer> players, IRefereeState state)
    {
      return players.Zip(state.AllPlayers)
        .Select(pair => new ProtectedPlayer(pair.Item1, pair.Item2, state, _responseTimeout))
        .ToList();
    }

    private IList<ProtectedObserver> ProtectObservers(IList<IObserver> observers)
    {
      return observers.Select(o => new ProtectedObserver(o, _responseTimeout)).ToList();
    }

    private (IList<ProtectedPlayer> winningPlayers, IList<ProtectedPlayer> misbehavedPlayers) RunGameSafe(
      IRefereeState state, IList<ProtectedPlayer> players, IList<ProtectedObserver> observers)
    {
      IDictionary<IPlayerInfo, ProtectedPlayer> playerDict = state.AllPlayers.Zip(players)
        .ToDictionary(p => p.Item1, p => p.Item2);
      SetupPlayers(state, playerDict);
      UpdateObservers(observers, state);
      var result = RunGameInternal(state, playerDict, observers);
      NotifyGameOver(result.winners, result.losers, observers);
      var misbehavedPlayers = DetermineMisbehavedPlayers(playerDict, state.AllPlayers);
      var finalWinners = DetermineFinalWinners(result.winners, misbehavedPlayers);
      return (finalWinners, misbehavedPlayers);
    }

    private static IList<ProtectedPlayer> DetermineFinalWinners(IEnumerable<ProtectedPlayer> winners,
      IEnumerable<ProtectedPlayer> misbehavedPlayers)
    {
      var misbehavedSet = misbehavedPlayers.ToHashSet();
      return winners.Where(w => !misbehavedSet.Contains(w)).ToList();
    }

    private (IList<ProtectedPlayer> winners, IList<ProtectedPlayer> losers) RunGameInternal(
      IRefereeState state, IDictionary<IPlayerInfo, ProtectedPlayer> playerDict, IList<ProtectedObserver> observers)
    {
      bool gameOver = false;
      for (int roundIdx = 0; roundIdx < _maxRounds && !gameOver; roundIdx += 1)
      {
        gameOver = CompleteOneRound(state, playerDict, observers, roundIdx);
      }

      var winners = DetermineWinners(state);
      var losers = DetermineLosers(state, winners);

      return (
        winners.Select(w => playerDict[w]).ToList(),
        losers.Select(l => playerDict[l]).ToList()
      );
    }

    private static IList<IPlayerInfo> DetermineLosers(IRefereeState state, IList<IPlayerInfo> winners)
    {
      var winnerSet = winners.ToHashSet();
      return state.AllPlayers.Where(p => !winnerSet.Contains(p))
        .ToList();
    }

    private void NotifyGameOver(IList<ProtectedPlayer> winners, IList<ProtectedPlayer> losers,
      IList<ProtectedObserver> observers)
    {
      NotifyWinnersGameOver(winners);
      NotifyLosersGameOver(losers);
      NotifyObserversGameOver(observers);
    }

    private void NotifyWinnersGameOver(IList<ProtectedPlayer> winners)
    {
      NotifyPlayersGameOver(winners, true);
    }

    private void NotifyLosersGameOver(IList<ProtectedPlayer> losers)
    {
      NotifyPlayersGameOver(losers, false);
    }

    private void NotifyObserversGameOver(IList<ProtectedObserver> observers)
    {
      observers.ForEach(o => o.NotifyGameOver());
    }

    private void NotifyPlayersGameOver(IList<ProtectedPlayer> players, bool won)
    {
      players.ForEach(p => p.Won(won));
    }

    private bool CompleteOneRound(IRefereeState state, IDictionary<IPlayerInfo, ProtectedPlayer> playerDict,
      IList<ProtectedObserver> observers, int roundIdx)
    {
      int numberOfPlayers = state.AllPlayers.Count();
      int numberOfPasses = 0;
      IPlayerInfo lastActivePlayer = state.ActivePlayer;
      for (int i = 0; i < numberOfPlayers; i += 1)
      {
        lastActivePlayer = state.ActivePlayer;
        RequestMoveFromActivePlayer(state, playerDict).IfSome(maybeMove =>
        {
          maybeMove.Match(
            _ =>
            {
              numberOfPasses += 1;
              state.NextTurn();
            },
            move => HandleMove(state, move, playerDict[state.ActivePlayer], roundIdx)
          );
        });
        UpdateObservers(observers, state);
        if (HasLastActivePlayerReachedHome(lastActivePlayer, roundIdx))
        {
          return true;
        }
      }

      return IsGameOver(lastActivePlayer, numberOfPasses, state.AllPlayers.Count(), roundIdx);
    }

    private void UpdateObservers(IList<ProtectedObserver> observers, IRefereeState state)
    {
      observers.ForEach(o => o.PushState(state));
    }

    private bool IsGameOver(IPlayerInfo lastActivePlayer, int numberOfPasses, int numberOfRemainingPlayers,
      int roundIdx)
    {
      return HasLastActivePlayerReachedHome(lastActivePlayer, roundIdx) || numberOfRemainingPlayers == 0 ||
             numberOfRemainingPlayers == numberOfPasses;
    }

    private void HandleMove(IRefereeState state, IMove move, ProtectedPlayer player, int roundIdx)
    {
      if (!IsValidMove(state, move))
      {
        state.RemoveActivePlayer();
        return;
      }

      PerformMove(state, move);
      if (HasActivePlayerCapturedAssignedTreasure(state))
      {
        HandlePlayerReachTreasure(state, player, roundIdx);
      }
      else
      {
        state.NextTurn();
      }
    }

    private static bool HasActivePlayerCapturedAssignedTreasure(IRefereeState state)
    {
      return state.HasActivePlayerReachedTreasure() && state.ActivePlayer.CapturedAllAssignedTreasuresRoundIdx.IsNone;
    }

    private static void HandlePlayerReachTreasure(IRefereeState state, ProtectedPlayer player, int roundIdx)
    {
      IPlayerInfo activePlayer = state.ActivePlayer;
      BoardPosition nextGoal = activePlayer.HomePosition;
      activePlayer.NumberOfCapturedTreasures += 1;
      if (state.NumberOfTreasures == 1 &&
          FindTreasurePosition(state.Board, state.NextTreasure).Equals(activePlayer.HomePosition))
      {
        state.PopNextTreasure();
        activePlayer.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(roundIdx);
      }
      else if (state.NumberOfTreasures > 0)
      {
        activePlayer.CurrentlyAssignedTreasure = state.PopNextTreasure();
        nextGoal = FindTreasurePosition(state.Board, activePlayer.CurrentlyAssignedTreasure);
      }
      else
      {
        activePlayer.CapturedAllAssignedTreasuresRoundIdx = Option<int>.Some(roundIdx);
      }

      player.Setup(Option<IPlayerState>.None, nextGoal)
        .IfSome(_ => state.NextTurn());
    }

    private Option<Either<IMove, Pass>> RequestMoveFromActivePlayer(IRefereeState state,
      IDictionary<IPlayerInfo, ProtectedPlayer> playerDict)
    {
      ProtectedPlayer player = playerDict[state.ActivePlayer];
      return player.TakeTurn(state.ToPlayerState());
    }

    private static IList<IPlayerInfo> DetermineWinners(IRefereeState state)
    {
      if (!state.AllPlayers.Any())
      {
        return new List<IPlayerInfo>();
      }

      var candidates = FindPlayersWhoCapturedMaxNumOfTreasures(state);
      var winners = FindPlayersWhoHaveMinDistanceToNextGoal(candidates, state.Board);
      return winners;
    }

    // Finds the maximum number of captured treasures among all the active players
    private static int MaxNumberOfCapturedTreasure(IRefereeState state)
    {
      return state.AllPlayers.Select(p => p.NumberOfCapturedTreasures).Max();
    }

    // Finds all active players who captured the maximum number of treasures in the game
    private static IList<IPlayerInfo> FindPlayersWhoCapturedMaxNumOfTreasures(IRefereeState state)
    {
      int numOfTreasures = MaxNumberOfCapturedTreasure(state);
      return state.AllPlayers.Where(p => p.NumberOfCapturedTreasures == numOfTreasures).ToList();
    }

    // Determines the given player's Euclidean distance to their next "goal", which is either a home, or a treasure
    // coordinate.
    private static int DistanceToNextGoal(IPlayerInfo playerInfo, IImmutableBoard board)
    {
      BoardPosition goal = playerInfo.CapturedAllAssignedTreasuresRoundIdx.IsSome
        ? playerInfo.HomePosition
        : FindTreasurePosition(board, playerInfo.CurrentlyAssignedTreasure);
      return playerInfo.CurrentPosition.EuclidDistance(goal);
    }

    // Finds the minimum distance to next goal (either home or coordinate of next assigned treasure) among all active
    // players
    private static int MinDistanceToNextGoal(IList<IPlayerInfo> playerInfos, IImmutableBoard board)
    {
      return playerInfos.Select(p => DistanceToNextGoal(p, board)).Min();
    }

    // Finds all the players who share minimum distance to their next goal
    private static IList<IPlayerInfo> FindPlayersWhoHaveMinDistanceToNextGoal(IList<IPlayerInfo> playerInfos,
      IImmutableBoard board)
    {
      int minDistance = MinDistanceToNextGoal(playerInfos, board);
      return playerInfos.Where(p => DistanceToNextGoal(p, board) == minDistance).ToList();
    }

    // Has any player reached home after capturing all of their assigned treasure(s) in a previous round?
    private static bool HasLastActivePlayerReachedHome(IPlayerInfo lastActivePlayer, int currentRoundIdx)
    {
      return
        lastActivePlayer.CurrentPosition.Equals(lastActivePlayer.HomePosition) &&
        lastActivePlayer.CapturedAllAssignedTreasuresRoundIdx.Match(
          roundIdx => currentRoundIdx > roundIdx,
          () => false
        );
    }

    private static BoardPosition FindTreasurePosition(IImmutableBoard board, ITreasure treasure)
    {
      return board.Positions.First(p => board.GetTileAt(p).Treasure.Equals(treasure));
    }

    private static IList<ProtectedPlayer> DetermineMisbehavedPlayers(
      IDictionary<IPlayerInfo, ProtectedPlayer> playerDict, IEnumerable<IPlayerInfo> survivingPlayers)
    {
      var survivingPlayerSet = survivingPlayers.ToHashSet();
      return playerDict.Keys.Where(p => !survivingPlayerSet.Contains(p))
        .Select(p => playerDict[p])
        .ToList();
    }

    private static void SetupPlayers(IRefereeState state, IDictionary<IPlayerInfo, ProtectedPlayer> playerDict)
    {
      playerDict.Keys.ForEach(internalPlayer =>
      {
        ProtectedPlayer player = playerDict[internalPlayer];
        BoardPosition goal = FindTreasurePosition(state.Board, internalPlayer.CurrentlyAssignedTreasure);
        player.Setup(Option<IPlayerState>.Some(state.ToPlayerState()), goal);
      });
    }

    private static void PerformMove(IRefereeState state, IMove move)
    {
      state.RotateSpareTile(move.Rotation);
      state.PerformSlide(move.Slide);
      state.MoveActivePlayer(move.Destination);
    }

    private bool IsValidMove(IRefereeState state, IMove move)
    {
      try
      {
        return _rule.ValidateMove(state.ToPlayerState(), move).IsLeft;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private abstract class ProtectedEntity
    {
      private readonly int _timeout;

      protected ProtectedEntity(int timeout)
      {
        _timeout = timeout;
      }

      private protected Option<T> ToOption<T>(Request<T> request)
      {
        try
        {
          using var tokenSource = new CancellationTokenSource();
          Task<T> task = Task.Run(request.Invoke, tokenSource.Token);
          if (task.Wait(TimeSpan.FromMilliseconds(_timeout)))
          {
            return task.Result;
          }

          tokenSource.Cancel();
          return Option<T>.None;
        }
        catch (Exception)
        {
          return Option<T>.None;
        }
      }
    }

    private sealed class ProtectedPlayer : ProtectedEntity
    {
      private readonly IPlayer _player;
      private readonly IPlayerInfo _playerInfo;
      private readonly IRefereeState _state;

      public ProtectedPlayer(IPlayer player, IPlayerInfo playerInfo, IRefereeState state, int timeout) :
        base(timeout)
      {
        _player = player;
        _playerInfo = playerInfo;
        _state = state;
      }

      // ReSharper disable once UnusedMember.Local
      public Option<string> Name
      {
        get
        {
          Request<string> request = () => _player.Name;
          Option<string> maybeName = ToOption(request);
          maybeName.IfNone(() => _state.RemovePlayer(_playerInfo));
          return maybeName;
        }
      }

      // ReSharper disable once UnusedMember.Local
      public Option<IBoard> ProposeBoard(int rows, int columns)
      {
        Request<IBoard> request = () => _player.ProposeBoard(rows, columns);
        Option<IBoard> maybeBoard = ToOption(request);
        maybeBoard.IfNone(() => _state.RemovePlayer(_playerInfo));
        return maybeBoard;
      }

      public Option<Acknowledge> Setup(Option<IPlayerState> initialState, BoardPosition goal)
      {
        Request<Acknowledge> request = () => _player.Setup(initialState, goal);
        Option<Acknowledge> maybeAck = ToOption(request);
        maybeAck.IfNone(() => { _state.RemovePlayer(_playerInfo); });
        return maybeAck;
      }

      public Option<Either<IMove, Pass>> TakeTurn(IPlayerState state)
      {
        Request<Either<IMove, Pass>> request = () => _player.TakeTurn(state);
        Option<Either<IMove, Pass>> maybeMove = ToOption(request);
        maybeMove.IfNone(() => { _state.RemovePlayer(_playerInfo); });
        return maybeMove;
      }

      public void Won(bool won)
      {
        Request<Acknowledge> request = () => _player.Won(won);
        Option<Acknowledge> maybeAck = ToOption(request);
        maybeAck.IfNone(() => { _state.RemovePlayer(_playerInfo); });
      }

      public IPlayer GetPlayerUnsafe()
      {
        return _player;
      }
    }

    private sealed class ProtectedObserver : ProtectedEntity
    {
      private readonly IObserver _observer;
      private bool _misbehaved;

      public ProtectedObserver(IObserver observer, int timeout) : base(timeout)
      {
        _observer = observer;
        _misbehaved = false;
      }

      public void PushState(IRefereeState state)
      {
        if (_misbehaved)
        {
          return;
        }

        Request<Acknowledge> request = () => _observer.PushState(state.Copy());
        var maybeAck = ToOption(request);
        maybeAck.IfNone(() => _misbehaved = true);
      }

      public void NotifyGameOver()
      {
        if (_misbehaved)
        {
          return;
        }

        Request<Acknowledge> request = () => _observer.NotifyGameOver();
        var maybeAck = ToOption(request);
        maybeAck.IfNone(() => _misbehaved = true);
      }
    }
  }

  public delegate T Request<T>();
}