
using Microsoft.EntityFrameworkCore;
using SharpCardAPI.Data;
using SharpCardAPI.Utility;

namespace SharpCardAPI.Tasks;

public class LoadDataTask : BackgroundService
{
    private readonly ILogger<LoadDataTask> _logger;
   

    public LoadDataTask(ILogger<LoadDataTask> logger){
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.Log(LogLevel.Information, "Checking database for questions");
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.UseSqlite("Data Source=db.sqlite3");
        var _dbContext = new AppDbContext(optionBuilder.Options);
        int questionsCount = _dbContext.Questions.ToList().Count;
        if (questionsCount > 0){
            _logger.LogInformation($"{questionsCount} Questions already loaded into database");
            return;
        }
        _logger.Log(LogLevel.Information, "Loading questions from csv files");
        string[] files = Directory.GetFiles("./packs");
        if (files.Length < 1){
            _logger.LogError("Failed to locate the folder with the csv files");
            return;
        }
        foreach(string file in files){
            await IOMethods.ReadQuestionCSV(_dbContext, file);
        }
        _logger.Log(LogLevel.Information, "Loaded questions from csv");
    }
}