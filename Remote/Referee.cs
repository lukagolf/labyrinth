using System;
using System.IO;
using System.Threading.Tasks;
using OneOf;
using Players;

namespace Remote
{
  /// <summary>
  /// Reads referee requests from a proxy player and forwards the requests to the actual player by calling
  /// the correct method on the player.
  /// </summary>
  public interface IPlayerDispatcher
  {
    /// <summary>
    /// Continuously listens for incoming requests and calls methods on the player based on the requests until won
    /// request is received.
    /// </summary>
    /// <param name="reader">The reader to use when reading remote referee method calls</param>
    /// <param name="writer">The writer to use when writing method returns values back to the referee</param>
    /// <param name="player">The player to call methods on</param>
    /// <returns>The game result or one of three errors</returns>
    public Task<Result> RunAsync(TextReader reader, TextWriter writer, IPlayer player);
  }

  /// <summary>
  /// Represents the result of a player-dispatcher run. It is either a bool indicating whether the player won or lost,
  /// or it is one of these three errors:
  /// - ReadError -> indicates that there's no more incoming requests from proxy player even though the won request
  ///   is not received.
  /// - WriteError -> indicates that writing a response to a proxy player failed.
  /// - PlayerError -> indicates that the local player reference thrown an exception.
  /// </summary>
  public sealed class Result
  {
    public Result(OneOf<ReadError, WriteError, PlayerError, bool> value)
    {
      Value = value;
    }

    public OneOf<ReadError, WriteError, PlayerError, bool> Value { get; }
  }

  public sealed class ReadError
  {
  }

  public sealed class WriteError
  {
    public WriteError(Exception exception)
    {
      Exception = exception;
    }

    public Exception Exception { get; }
  }

  public sealed class PlayerError
  {
    public PlayerError(Exception exception)
    {
      Exception = exception;
    }

    public Exception Exception { get; }
  }
}