namespace SharpCardAPI.Models;


public class Question{
    public int QuestionId { get; set; }
    public ProblemSolution Solution { get; set; } = null!;
    public string ProblemStatement { get; set; } = string.Empty;
    public string ProblemTitle { get; set; } = string.Empty;
    public string Problem { get; set; } = string.Empty;
    public string ProblemUrl { get; set; } = string.Empty;
    public ICollection<Option> Options { get; } = new List<Option>();
    public ICollection<QuestionTag> Tags { get; } = new List<QuestionTag>();
}

