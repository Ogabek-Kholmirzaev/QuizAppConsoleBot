using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBotQuizApp.Services
{
    public class TelegramBotService
    {
        private string Token = "5778067511:AAEroiW88hCmc3HVqhi-Z9IEdjXjbbfIkFA";
        public TelegramBotClient Bot;

        public TelegramBotService()
        {
            Bot = new TelegramBotClient(Token);
        }

        public void GetUpdate(Func<ITelegramBotClient, Update, CancellationToken, Task> update)
        {
            Bot.StartReceiving(
                updateHandler: update,
                errorHandler: (_, ex, _) =>
                {
                    Console.WriteLine(ex.Message);
                    return Task.CompletedTask;
                });
        }
        public void SendMessage(long chatId, string message, IReplyMarkup? reply = null)
        {
            Bot.SendTextMessageAsync(chatId, message, replyMarkup: reply);
        }
    }
}
