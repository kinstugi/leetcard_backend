using SharpCardAPI.Data;
using SharpCardAPI.Models;

namespace SharpCardAPI.Utility;

public class IOMethods{
    public static async Task ReadQuestionCSV(AppDbContext context){
        
        using (var parser = new StreamReader("questions.tsv"))
        {
            List<Question> questionList = new List<Question>();
            int i = 0, batchCount = 0, batchSize = 50;
            while(!parser.EndOfStream)
            {
                i += 1;
                string line = parser.ReadLine()!;
                if (i == 1) continue;
                string[] cols = line.Split('\t');
                var solution = new ProblemSolution {TimeComplexity = cols[3], SpaceComplexity = cols[4], SolutionUrl = cols[12]};
                var question = new Question{ProblemTitle = cols[0], ProblemStatement = cols[2], Problem = cols[5], ProblemUrl = cols[1]};
                question.Solution = solution;
                question.Options.Add(new Option{Text = cols[6], Message = cols[13], IsCorrect = 6 == GetCorrectIndex(cols[10])});
                question.Options.Add(new Option{Text = cols[7], Message = cols[14], IsCorrect = 7 == GetCorrectIndex(cols[10])});
                question.Options.Add(new Option{Text = cols[8], Message = cols[15], IsCorrect = 8 == GetCorrectIndex(cols[10])});
                question.Options.Add(new Option{Text = cols[9], Message = cols[17], IsCorrect = 9 == GetCorrectIndex(cols[10])});
                questionList.Add(question);

                if ((++batchCount % batchSize) == 0){
                    await context.Questions.AddRangeAsync(questionList);
                    await context.SaveChangesAsync();
                    questionList.Clear();
                }
            }
            if (questionList.Count > 0){
                await context.Questions.AddRangeAsync(questionList);
                await context.SaveChangesAsync();
            }
        }
    }

    private static int GetCorrectIndex(string ch, int offset = 6){
        return ch[0]-'A' + offset;
    }

}