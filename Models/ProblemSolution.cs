namespace SharpCardAPI.Models;

public class ProblemSolution{
    public int ProblemSolutionId { get; set; }
    public string TimeComplexity { get; set; } = string.Empty;
    public string SpaceComplexity { get; set; } = string.Empty;
    public string SolutionUrl { get; set; } = string.Empty;
    public int QuestionId { get; set; }
}