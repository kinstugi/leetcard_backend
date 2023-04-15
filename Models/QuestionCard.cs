namespace SharpCardAPI.Models;
// to get the 


public class QuestionCard{
    private const double pUnanswered = 0.7;
    private const double pIncorrect = 0.2; 
    private const double pCorrect = 0.1;

    public int QuestionCardId { get; set; }
    public Question Question { get; set; } = null!;
    public User User { get; set; } = null!;
    public bool DidAttempt { get; set; } = false;
    public int DifficultyLevel { get; set; }
    public DateTime LastReviewed { get; set; }
    public ICollection<SolutionHistory> Histories { get; } = new List<SolutionHistory>();

    public int GetQuestionCardWeight(){
        switch (DifficultyLevel)
        {
            case 0:
                return (int)(1000 * pUnanswered);
            case 1:
                return (int)(pIncorrect * (1 + DifficultyLevel) * 1000);
            case 2:
                return (int)(pCorrect * (1 - DifficultyLevel) * 1000);   
            default:
                return 0;
        }
    }

    public void UpdateQuestionCard(bool answeredCorrectly){
        if (answeredCorrectly){
            DifficultyLevel = Math.Max(0, DifficultyLevel - 1);
        }else{
            DifficultyLevel = Math.Min(2, DifficultyLevel + 1);
        }
        LastReviewed = DateTime.Now;
    }
}
