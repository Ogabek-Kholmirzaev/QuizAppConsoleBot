namespace ConsoleBotQuizApp.Models
{
    public class Result
    {
        public string Name;
        public int CorrectAnswers;
        public int TotalAnswers;
        public int Percent;

        public Result(string name, int correctAnswers, int totalAnswers)
        {
            Name = name;
            CorrectAnswers = correctAnswers;
            TotalAnswers = totalAnswers;
            Percent = (int)((double)correctAnswers / (double)totalAnswers * 100);
        }
    }
}
