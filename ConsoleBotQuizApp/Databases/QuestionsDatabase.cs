using ConsoleBotQuizApp.Models;

namespace ConsoleBotQuizApp.Databases
{
    public class QuestionsDatabase
    {
        public List<Question> Questions = new List<Question>();

        public void AddDefaultQuestions()
        {
            Questions.Add(new Question("2 + 2 * 2 = ?", 1, new List<string>(){"8", "6", "5", "7"}));
            Questions.Add(new Question("8 * 7 = ?", 2, new List<string>() { "52", "54", "56", "58" }));
            Questions.Add(new Question("3 + 7 = ?", 0, new List<string>() { "10", "11", "9"}));
        }

        public bool AddQuestion(string message)
        {
            string[] questionList = message.Split(',').ToArray();

            if (questionList.Length <= 4) return false;

            Questions.Add(new Question(questionList[0], int.Parse(questionList[1]), questionList.Skip(2).ToList()));
            return true;
        }

        public Question GetQuestion(int index)
        {
            return Questions[index];
        }

        public string GetQuestionsText()
        {
            string text = "";

            for (int i = 0; i < Questions.Count; i++)
            {
                text += $"{i + 1}. {Questions[i].QuestionText} \n";
            }

            return text;
        }
    }
}
