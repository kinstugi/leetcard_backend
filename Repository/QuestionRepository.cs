using SharpCardAPI.Data;
using SharpCardAPI.Models;

namespace SharpCardAPI.Repository;

public class QuestionRepository{
    private readonly AppDbContext _context;
    public QuestionRepository(AppDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<Question>> GetAll(){
        return await Task.Run(() => _context.Questions.ToList());
    }

    public async Task<bool> UpdateQuestionCard(int userId, int questionCardId, bool answer){
        var card = await Task.Run(()=> _context.QuestionCards.Where(c => c.User.UserId == userId && c.QuestionCardId == questionCardId).FirstOrDefault());
        if (card == null) throw new Exception("not found");
        card.UpdateQuestionCard(answer);
        await _context.SaveChangesAsync();
        return false;
    }
}