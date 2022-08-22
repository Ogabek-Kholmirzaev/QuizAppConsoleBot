using ConsoleBotQuizApp.Models;

namespace ConsoleBotQuizApp.Databases
{
    public class ResultsDatabase
    {
        public List<Result> Results = new List<Result>();

        public string Rating()
        {
            string message = "";
            var results = Results.OrderByDescending(r => r.Percent).ToList();

            for (int i = 0; i < results.Count; i++)
                message += $"{i + 1}.    {results[i].Name}    {results[i].CorrectAnswers}/{results[i].TotalAnswers}    {results[i].Percent}%\n";

            return message;
        }
    }
}
