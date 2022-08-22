namespace ConsoleBotQuizApp.Models
{
    public class User
    {
        public long ChatId;
        public string Name;
        public int Step;
        public int QuestionIndex;
        public int CorrectAnswers;

        public User(long chatId, string name)
        {
            ChatId = chatId;
            Name = name;
            Step = 0;
            QuestionIndex = 0;
            CorrectAnswers = 0;
        }

        public User(long chatId, string name, int step)
        {
            ChatId = chatId;
            Name = name;
            Step = step;
            QuestionIndex = 0;
            CorrectAnswers = 0;
        }

        public void SetStep(int step)
        {
            Step = step;
        }

        public string ToText()
        {
            return $"{ChatId} {Name} {Step}";
        }
    }
}
