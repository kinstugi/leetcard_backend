using Microsoft.EntityFrameworkCore;
using SharpCardAPI.Models;

namespace SharpCardAPI.Data;
public class AppDbContext: DbContext{
    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionCard> QuestionCards { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<ProblemSolution> ProblemSolutions { get; set; }
    public DbSet<SolutionHistory> SolutionHistories { get; set; }
    public DbSet<QuestionTag> QuestionTags { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Users = Set<User>();
        Questions = Set<Question>();
        QuestionCards = Set<QuestionCard>();
        Options = Set<Option>();
        ProblemSolutions = Set<ProblemSolution>();
        SolutionHistories = Set<SolutionHistory>();
        QuestionTags = Set<QuestionTag>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        optionsBuilder.UseSqlite("Data Source=db.sqlite3");
    }
}
