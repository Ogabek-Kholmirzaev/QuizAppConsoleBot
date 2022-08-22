namespace ConsoleBotQuizApp.Models
{
    public class Question
    {
        public string QuestionText;
        public int CorrectAnswerIndex;
        public List<String> Choices;

        public Question(string questionText, int correctAnswerIndex, List<string> choices)
        {
            QuestionText = questionText;
            CorrectAnswerIndex = correctAnswerIndex;
            Choices = choices;
        }
    }
}
