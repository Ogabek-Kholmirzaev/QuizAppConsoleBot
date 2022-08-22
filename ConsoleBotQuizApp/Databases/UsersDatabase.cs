using System.Data;
using ConsoleBotQuizApp.Models;

namespace ConsoleBotQuizApp.Databases
{
    public class UsersDatabase
    {
        public List<User> Users = new List<User>();

        public User AddUser(long chatId, string name)
        {
            var user = new User(chatId, name);
            Users.Add(user);
            return user;
        }

        public User? GetUser(long chatId, string name)
        {
            if (Users.Exists(user => user.ChatId == chatId))
                return Users.FirstOrDefault(user => user.ChatId == chatId);
           
            return AddUser(chatId, name);
        }

        public string Rating()
        {
            string message = "";

            for (int i = 0; i < Users.Count; i++) 
                message += $"{i + 1}. {Users[i].Name}";

            return message;
        }

        public string GetUsersText()
        {
            string message = "";

            for (int i = 0; i < Users.Count; i++)
                message += $"{i + 1}. Id: {Users[i].ChatId}    Name: {Users[i].Name}     Step: {Users[i].Step}\n";

            return message;
        }
    }
}
