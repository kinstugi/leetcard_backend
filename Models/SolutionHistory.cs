namespace SharpCardAPI.Models;


public enum SolutionState{
    Optimal,
    Wrong,
    Unsolved
}

public class SolutionHistory{
    public int SolutionHistoryId { get; set; }
    public SolutionState SolutionState { get; set; } = SolutionState.Unsolved;
    public DateTime LastSeen { get; set; }
    public int QuestionCardId { get; set; }
}