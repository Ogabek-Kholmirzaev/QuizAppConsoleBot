using ConsoleBotQuizApp.Databases;
using ConsoleBotQuizApp.Services;
using System;
using ConsoleBotQuizApp.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Reflection;

var db = new Database();
var bot = new TelegramBotService();

bot.GetUpdate((_, update, _) => GetUpdate(update));
db.QuestionsDb.AddDefaultQuestions();

Console.ReadKey();

async Task GetUpdate(Update update)
{
    if (update.Type != UpdateType.Message) return;

    Console.WriteLine(update.Message.Chat.Id + " " + update.Message.Text!);

    var message = update.Message!.Text;
    var chatId = update.Message!.Chat.Id;
    var name = update.Message.From.Username == null
        ? update.Message.From.FirstName
        : "@" + update.Message.From.Username;

    var user = db.UsersDb.GetUser(chatId, name);

    if (message == "Menu")
    {
        ShowMenu(user);
        return;
    }

    switch (user.Step)
    {
        case 0:
            ShowMenu(user);
            break;
        case 1:
            SwitchMenu(user, message, name);
            break;
        case 2:
            AddQuestion(user, message);
            break;
        case 3:
            Statistics(user, message, name);
            break;
        case 4:
            StartQuiz(user, message, name);
            break;
    }

}

void ShowMenu(ConsoleBotQuizApp.Models.User user)
{
    List<Enum> menu = new List<Enum>()
    {
        EMenu.StartQuiz,
        EMenu.AddQuestion,
        EMenu.Dashboard,
        EMenu.Statistics,
        EMenu.Users,
        EMenu.Close
    };

    var menuButtons = new KeyboardButton[menu.Count][];

    for (int i = 0; i < menu.Count; i++)
        menuButtons[i] = new[] { new KeyboardButton(menu[i].ToString()) };

    Console.WriteLine(user.ChatId + " " + user.Name + " " + user.Step);

    user.SetStep(1);
    bot.SendMessage(user.ChatId, "Menuni tanlang 👇", new ReplyKeyboardMarkup(menuButtons) { ResizeKeyboard = true });
}

void SwitchMenu(ConsoleBotQuizApp.Models.User user, string message, string name)
{
    switch (message)
    {
        case "StartQuiz": ShowStartQuiz(user, name); break;
        case "AddQuestion": ShowAddQuestion(user); break;
        case "Dashboard": ShowDashboard(user); break;
        case "Statistics": ShowStatistics(user); break;
        case "Users": ShowUsers(user); break;
        case "Close": ShowClose(user); break;
        default: bot.SendMessage(user.ChatId, "Menuni tanlang"); break;

    }
}

void ShowStartQuiz(ConsoleBotQuizApp.Models.User user, string name)
{
    int index = user.QuestionIndex;

    var question = db.QuestionsDb.GetQuestion(index);
    var menuButtons = new KeyboardButton[question.Choices.Count][];

    for (int i = 0; i < question.Choices.Count; i++)
        menuButtons[i] = new[] { new KeyboardButton(question.Choices[i]) };

    user.SetStep(4);
    bot.SendMessage(user.ChatId, $"{index + 1} - savol\n{question.QuestionText}", new ReplyKeyboardMarkup(menuButtons){ResizeKeyboard = true});
}

void StartQuiz(ConsoleBotQuizApp.Models.User user, string message, string name)
{
    var question = db.QuestionsDb.Questions[user.QuestionIndex];

    if(message == question.Choices[question.CorrectAnswerIndex])
    {
        user.CorrectAnswers++;
        bot.SendMessage(user.ChatId, "To'g'ri ✅");
    }
    else bot.SendMessage(user.ChatId, "Noto'g'ri ❌");

    user.QuestionIndex++;

    if (user.QuestionIndex == db.QuestionsDb.Questions.Count)
    {
        user.SetStep(1);

        db.ResultsDb.Results.Add(new Result(name, user.CorrectAnswers, db.QuestionsDb.Questions.Count));

        var menuButton = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("Menu") } }) { ResizeKeyboard = true };
        bot.SendMessage(user.ChatId, $"Savollar tugadi\nTo'g'ri javoblar soni {user.CorrectAnswers} ta", menuButton);

        user.QuestionIndex = 0;
        user.CorrectAnswers = 0;
    }
    else ShowStartQuiz(user, name);
}

void ShowAddQuestion(ConsoleBotQuizApp.Models.User user)
{
    user.SetStep(2);

    var message = "Savolni quyidagi tartibda kiriting : \n 1 + 4 = ?, 2, 12, 14, 5, 6";
    var menuButtons = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("Menu") } }) { ResizeKeyboard = true };

    bot.SendMessage(user.ChatId, message, menuButtons);
}

void AddQuestion(ConsoleBotQuizApp.Models.User user, string message)
{
    if (db.QuestionsDb.AddQuestion(message) == false) { ShowAddQuestion(user); return; }

    user.SetStep(1);
    bot.SendMessage(user.ChatId, "Savol qo'shildi ✅");
}

void ShowDashboard(ConsoleBotQuizApp.Models.User user)
{
    string text = $"Jami savollar soni {db.QuestionsDb.Questions.Count} ta \n";
    text += db.QuestionsDb.GetQuestionsText();

    bot.SendMessage(user.ChatId, text);

    user.SetStep(1);
}

void ShowStatistics(ConsoleBotQuizApp.Models.User user)
{
    var menuButtons = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton($"{EMenu.Show.ToString()}")},
                new[] { new KeyboardButton($"{EMenu.Clear.ToString()}")},
                new[] { new KeyboardButton("Menu")}
            }) { ResizeKeyboard = true };
    
    bot.SendMessage(user.ChatId, "Statistikani tanlang 👇", menuButtons);
    
    user.SetStep(3);
}

void Statistics(ConsoleBotQuizApp.Models.User user, string message, string name)
{
    var menuButtons = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("Menu") } }) { ResizeKeyboard = true };
    var text = "";

    if (message == "Show") text = db.ResultsDb.Rating();
    else
    {
        db.UsersDb.Users.Clear();
        user = db.UsersDb.AddUser(user.ChatId, name);
        text = "Tozalandi.";
    }

    bot.SendMessage(user.ChatId, text, menuButtons);
    user.SetStep(1);
}

void ShowUsers(ConsoleBotQuizApp.Models.User user)
{
    var menuButtons = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("Menu") } }) { ResizeKeyboard = true };

    user.SetStep(1);
    bot.SendMessage(user.ChatId, db.UsersDb.GetUsersText(), menuButtons);
}

void ShowClose(ConsoleBotQuizApp.Models.User user)
{
    var menuButtons = new ReplyKeyboardMarkup(new[] { new[] { new KeyboardButton("Menu") } }) { ResizeKeyboard = true };

    user.SetStep(1);
    bot.SendMessage(user.ChatId, $"Botdan foydalanganingiz uchun rahmat\nCreated by @BotFather", menuButtons);
}