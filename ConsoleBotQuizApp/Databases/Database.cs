namespace ConsoleBotQuizApp.Databases
{
    public class Database
    {
        public QuestionsDatabase QuestionsDb;
        public UsersDatabase UsersDb;
        public ResultsDatabase ResultsDb;

        public Database()
        {
            QuestionsDb = new QuestionsDatabase();
            UsersDb = new UsersDatabase();
            ResultsDb = new ResultsDatabase();
        }
    }
}
