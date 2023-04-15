using Microsoft.EntityFrameworkCore;
using SharpCardAPI.Data;
using SharpCardAPI.Models;

namespace SharpCardAPI.Repository;

public class UserRepository{
    private readonly AppDbContext _dbConext;

    public UserRepository(AppDbContext dbContext){
        _dbConext = dbContext;
    }

    public async Task CreateUserQuestionCards(User user) {
        var questions = await Task.Run(()=> _dbConext.Questions.ToList());
        int batchCount = 0, batchSize = 25;
        List<QuestionCard> questionCards = new List<QuestionCard>();
        foreach(var question in questions){
            batchCount++;
            questionCards.Add(new QuestionCard{ Question = question, User = user});
            if (batchCount % batchSize == 0){
                await _dbConext.QuestionCards.AddRangeAsync(questionCards);
                await _dbConext.SaveChangesAsync();
                questionCards.Clear();
            }
        }
        if (questionCards.Any()){
            await _dbConext.QuestionCards.AddRangeAsync(questionCards);
            await _dbConext.SaveChangesAsync();
        }
    }

    public async Task<List<QuestionCard>> GetUserQuestionCards(int userId){
        var mycards = await Task.Run(()=> _dbConext.QuestionCards
            .Where( u => u.User.UserId == userId)
            .Include(qCard => qCard.Question)
            .ThenInclude(card => card.Options).ToList());
        return mycards;
    }
}