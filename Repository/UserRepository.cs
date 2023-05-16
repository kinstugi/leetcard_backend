using Microsoft.EntityFrameworkCore;
using SharpCardAPI.Data;
using SharpCardAPI.Models;

namespace SharpCardAPI.Repository;

public class UserRepository{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext){
        _dbContext = dbContext;
    }

    public async Task CreateUserQuestionCards(User user) {
        var questions = await Task.Run(()=> _dbContext.Questions.ToList());
        int batchCount = 0, batchSize = 25;
        List<QuestionCard> questionCards = new List<QuestionCard>();
        foreach(var question in questions){
            batchCount++;
            questionCards.Add(new QuestionCard{ Question = question, User = user});
            if (batchCount % batchSize == 0){
                await _dbContext.QuestionCards.AddRangeAsync(questionCards);
                await _dbContext.SaveChangesAsync();
                questionCards.Clear();
            }
        }
        if (questionCards.Any()){
            await _dbContext.QuestionCards.AddRangeAsync(questionCards);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<QuestionCard>> GetUserQuestionCards(int userId, string pack){
        var mycards = await Task.Run(()=> _dbContext.QuestionCards
            .Where( u => u.User.UserId == userId)
            .Where(q => q.Question.Pack == pack)
            .Include(qCard => qCard.Question)
            .ThenInclude(card => card.Options)
            .Include(card => card.Question.Solution)
            .ToList());
        return mycards;
    }

       public async Task<double> GetUserStats(int userId){
        var totalAttempts = await Task.Run(()=> _dbContext.QuestionCards.Where(card => card.User.UserId == userId).Sum(card => card.NumberOfAttempts));
        var totalSuccess = await Task.Run(() => _dbContext.QuestionCards.Where(card => card.User.UserId == userId).Sum(card=>card.CorrectAttempts));
        if (totalAttempts == 0) return -1;
        var res =  (double)totalSuccess / totalAttempts;
        return Math.Round(res, 2);
    }

    public async Task<IEnumerable<Question>> GetTopQuestions(int userId, int count)
    {
        var totalAttempts = await Task.Run(() => _dbContext.QuestionCards.Count(card => card.User.UserId == userId));
        if (totalAttempts == 0)
        {
            return new List<Question>();
        }

        var topQuestions = await _dbContext.QuestionCards
            .Include(card => card.Question)
            .Where(card => card.NumberOfAttempts > 0)
            .GroupBy(card => card.Question)
            .Select(group => new
            {
                Question = group.Key,
                SuccessRate = (double)group.Sum(card => card.CorrectAttempts) / group.Sum(card => card.NumberOfAttempts)
            })
            .OrderByDescending(q => q.SuccessRate)
            .Take(count)
            .Select(q => q.Question)
            .ToListAsync();

        return topQuestions;
    }

    public async Task<IEnumerable<Question>> GetBottomQuestions(int userId, int count)
    {
        var totalAttempts = await Task.Run(() => _dbContext.QuestionCards.Count(card => card.User.UserId == userId));
        if (totalAttempts == 0)
        {
            return new List<Question>();
        }

        var bottomQuestions = await _dbContext.QuestionCards
            .Include(card => card.Question)
            .Where(card => card.NumberOfAttempts > 0)
            .GroupBy(card => card.Question)
            .Select(group => new
            {
                Question = group.Key,
                SuccessRate = (double)group.Sum(card => card.CorrectAttempts) / group.Sum(card => card.NumberOfAttempts)
            })
            .OrderBy(q => q.SuccessRate)
            .Take(count)
            .Select(q => q.Question)
            .ToListAsync();

        return bottomQuestions;
    }


    public async Task ResetData(int userId){
        var userCards = _dbContext.QuestionCards.Where(q => q.User.UserId == userId);
        foreach (var card in userCards)
        {
            card.NumberOfAttempts = 0;
            card.CorrectAttempts = 0;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<QuestionCard>> GetCardsToReview(int userId) {
        return await Task.Run(()=>_dbContext.QuestionCards
            .Where(q => q.User.UserId == userId)
            .Where(q => q.NextScheduledReview <= DateTime.Now)
            .Include(q => q.Question)
            .ThenInclude(q => q.Options)
            .ToList()
            );
    }

    public async Task<List<QuestionCard>> GetLearningCards(int userId, int count, string pack){
        return await Task.Run(() => _dbContext.QuestionCards
            .Where(q => q.User.UserId == userId)
            .Where(q => q.Repititions == 0 || q.NextScheduledReview <= DateTime.Now)
            .Where(q => q.Question.Pack == pack)
            .Include(q => q.Question)
            .ThenInclude(q => q.Options)
            .ToList()
            .GetRange(0, count)
        );
    }
}