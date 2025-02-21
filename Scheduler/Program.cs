using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Collections.Concurrent;
using StudyXLS.User;
using StudyXLS.Pages;
using Telegram.Bot.Types.Enums;

class Program
{
    private static UserStateManager _userStateManager;

    static async Task Main(string[] args)
    {

        var telegramClient = new TelegramBotClient(token: "7567444597:AAGTAeZ3tvitYv_CHqf0ZYhMy8fvh1TcIz8");
        _userStateManager = new UserStateManager(telegramClient);

        telegramClient.StartReceiving(updateHandler: HandleUpdate, errorHandler: HandleError,);

        Console.ReadLine();
    }

    private static async Task HandleError(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
    }

    private static async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken token)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await HandleMessage(client, update);
                break;
            
        }
    }

    private static async Task HandleMessage(ITelegramBotClient client, Update update)
    {
        var chatId = update.Message.Chat.Id;
        var messageId = update.Message.MessageId;
        var text = update.Message.Text;

        if (update.Message.Document != null)
        {

        }
        else
        {
            switch (text)
            {
                case "/start":
                    await _userStateManager.ShowPageAsync(update.Message.Chat.Id, new MainMenu());
                    break;

                case "Изменить расписание":
                    await _userStateManager.UpdatePageAsync(chatId, messageId, new ChangeSchedulePage());
                    break;
            }
        }     
    }
}