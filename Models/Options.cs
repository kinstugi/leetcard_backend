namespace SharpCardAPI.Models;

public class Option{
    public int OptionId { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsCorrect { get; set; } = false;
}