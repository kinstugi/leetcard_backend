namespace SharpCardAPI.Models;


public class SolutionHistory{
    public int SolutionHistoryId { get; set; }
    public bool SolutionState { get; set; }
    public DateTime LastSeen { get; set; }
    public int QuestionCardId { get; set; }
}