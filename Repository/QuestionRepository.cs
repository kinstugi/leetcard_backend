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
}