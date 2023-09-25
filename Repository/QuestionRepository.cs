using Microsoft.EntityFrameworkCore;
using SharpCardAPI.Data;
using SharpCardAPI.Models;
using SharpCardAPI.Utility;

namespace SharpCardAPI.Repository;
public interface IQuestionRepository{
    public Task<IEnumerable<Question>> GetAll(string pack);
    public Task<IEnumerable<Question>> GetRandomQuestion(string pack, int count=5);
}

public class QuestionRepository:IQuestionRepository{
    private readonly AppDbContext _context;
    public QuestionRepository(AppDbContext context){
        _context = context;
    }

    public async Task<IEnumerable<Question>> GetAll(string pack){
        return await Task.Run(() => _context
        .Questions
        .Include(q => q.Solution)
        .Include(q => q.Options)
        .Where(q => q.Pack == pack)
        .ToList());
    }

    public async Task<IEnumerable<Question>> GetRandomQuestion(string pack, int count=5){
        var questions = await GetAll(pack);
        return MiscMethods.SelectRandomDistinct<Question>(questions.ToList(), count);
    }
}