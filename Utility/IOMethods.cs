using SharpCardAPI.Data;
using SharpCardAPI.Models;

namespace SharpCardAPI.Utility;

public class IOMethods{
    public static async Task ReadQuestionCSV(AppDbContext context, string filePath){
        
 using (var parser = new StreamReader(filePath))
        {
            List<Question> questionList = new List<Question>();
            int i = 0, batchCount = 0, batchSize = 50;
            while (!parser.EndOfStream)
            {
                i += 1;
                string line = parser.ReadLine()!;
                if (i == 1) continue;
                string[] cols = line.Split('\t');
                var solution = new ProblemSolution { TimeComplexity = cols[5], SpaceComplexity = cols[6], VideoSolutionUrl = cols[21], CppCodeUrl = cols[18], PythonCodeUrl = cols[19], JavaCodeurl = cols[20] };
                var question = new Question { Pack = cols[0], ProblemTitle = cols[2], ProblemStatement = cols[4], Problem = cols[7], ProblemUrl = cols[3], Hint = cols[17], Topic = cols[1], VideoSolutionUrl = cols[21]};
                question.Solution = solution;
                question.Options.Add(new Option { Text = cols[8], Message = cols[13], IsCorrect = 8 == GetCorrectIndex(cols[12]) });
                question.Options.Add(new Option { Text = cols[9], Message = cols[14], IsCorrect = 9 == GetCorrectIndex(cols[12]) });
                question.Options.Add(new Option { Text = cols[10], Message = cols[15], IsCorrect = 10 == GetCorrectIndex(cols[12]) });
                question.Options.Add(new Option { Text = cols[11], Message = cols[16], IsCorrect = 11 == GetCorrectIndex(cols[12]) });
                questionList.Add(question);

                if ((++batchCount % batchSize) == 0)
                {
                    await context.Questions.AddRangeAsync(questionList);
                    await context.SaveChangesAsync();
                    questionList.Clear();
                }
            }
            if (questionList.Count > 0)
            {
                await context.Questions.AddRangeAsync(questionList);
                await context.SaveChangesAsync();
            }
        }
    }

    private static int GetCorrectIndex(string ch, int offset = 8){
        return ch[0]-'A' + offset;
    }
}