namespace SharpCardAPI.Models;

// i am thinking this can represent the category of a question, it can be Binary Search, DFS etc
public class QuestionTag{
    public int QuestionTagId { get; set; }
    public string Tag { get; set; } = string.Empty;
}