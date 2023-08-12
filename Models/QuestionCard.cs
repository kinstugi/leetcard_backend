using SharpCardAPI.Utility;

namespace SharpCardAPI.Models;

public class QuestionCard{
    public DateTime NextScheduledReview { get; set; } = DateTime.Now.AddYears(50);
    public int Repetitions { get; set; } = 0;
    public double EaseFactor { get; set; } = 1.3;

    public int NumberOfAttempts { get; set; } = 0;
    public int CorrectAttempts { get; set; } = 0;

    public int QuestionCardId { get; set; }
    public Question Question { get; set; } = null!;
    public User User { get; set; } = null!;

    public void UpdateQuestionCard(int quality){
        SM2Algorithm.CalculateNewInterval(Repetitions, NextScheduledReview, EaseFactor, quality, out DateTime nextScheduledReview, out double newEaseFactor, out int newRep);
        Repetitions = newRep;
        EaseFactor = newEaseFactor;
        NextScheduledReview = nextScheduledReview;
    }

    public void UpdateStatistics(bool answeredCorrectly){
        if (answeredCorrectly){
            CorrectAttempts++;
        }
        NumberOfAttempts++;
    }
}

public class QuestionResponseDTO{
    public int AnswerQuality { get; set; }
    public bool DidAnswerCorrectly { get; set; }
}