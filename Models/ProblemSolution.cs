namespace SharpCardAPI.Models;

public class ProblemSolution{
    public int ProblemSolutionId { get; set; }
    public string TimeComplexity { get; set; } = string.Empty;
    public string SpaceComplexity { get; set; } = string.Empty;
    public string VideoSolutionUrl { get; set; } = string.Empty;
    public string PythonCodeUrl { get; set; } = string.Empty;
    public string JavaCodeurl { get; set; } = string.Empty;
    public string CppCodeUrl { get; set; } = string.Empty;
    public int QuestionId { get; set; }
}