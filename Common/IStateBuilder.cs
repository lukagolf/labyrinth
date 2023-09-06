namespace Common
{
  public interface IRefereeStateBuilder
  {
    public IRefereeState BuildState(int boardHeight, int boardWidth, int numberOfPlayers);
    public IRefereeState BuildState(int numberOfPlayers);
  }
}