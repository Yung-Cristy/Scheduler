using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using System.Collections.Concurrent;
using Scheduler.Pages;
using Telegram.Bot.Types.Enums;
using Scheduler.User;
using Scheduler.Employee;

class Program
{
    private static UserStateManager _userStateManager;
    private static ConcurrentDictionary<long, UserData> _userDataCache = new ConcurrentDictionary<long, UserData>();
    private static EmployeeManager _employeesManager = new EmployeeManager();

    static async Task Main(string[] args)
    {

        var telegramClient = new TelegramBotClient(token: "7567444597:AAGTAeZ3tvitYv_CHqf0ZYhMy8fvh1TcIz8");
        _userStateManager = new UserStateManager(telegramClient);

        telegramClient.StartReceiving(updateHandler: HandleUpdate, errorHandler: HandleError);

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
            case UpdateType.CallbackQuery:
                await HandleCallbackQuery(client, update);
                break;

        }
    }

    private static async Task HandleCallbackQuery(ITelegramBotClient client, Update update)
    {
        var inputCallback = update.CallbackQuery.Data;
        var messageId = update.CallbackQuery.Message.Id;
        var chatId = update.CallbackQuery.Message.Chat.Id;
        var userId = update.CallbackQuery.From.Id;

        switch (inputCallback)
        {
            case "Изменить расписание":
                await _userStateManager.EditPageAsync(userId, messageId, new ChangeSchedulePage());
                break;
            case "Изменить список сотрудников":
                await _userStateManager.EditPageAsync(userId, messageId, new ChangeEmployeesPage());
                break;
            case "Добавить сотрудника":
                await _userStateManager.EditPageAsync(userId, messageId, new RequestNameOfNewEmployeePage());
                break;

        }
    }

    private static async Task HandleMessage(ITelegramBotClient client, Update update)
    {
        var chatId = update.Message.Chat.Id;
        var messageId = update.Message.MessageId;
        var text = update.Message.Text;
        var userId = update.Message.From.Id;

        if (text == "/start")
        {
            await _userStateManager.ShowPageAsync(userId, new MainMenu());
        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestNameOfNewEmployeePage)
        {
            _userDataCache.AddOrUpdate(
                key: userId,
                addValue: new UserData { Name = text },
                updateValueFactory: (x, y) =>
                    {
                        y.Name = text;
                        return y;
                    });
            await _userStateManager.EditPageAsync(userId, messageId, new RequestTelegramIdOfNewEmployeePage());

        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestTelegramIdOfNewEmployeePage)
        {
            _userDataCache[userId].TelegramId = userId;
            await _userStateManager.EditPageAsync(userId, messageId, new RequestDirectionOfNewEmployeePage());
        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestDirectionOfNewEmployeePage)
        {
            if (Enum.TryParse(text, out Direction direction))
            {
                // Если преобразование успешно, обновляем Direction
                _userDataCache[userId].Direction = direction;

                _employeesManager.AddEmployee(
                    employee: new Employee(
                        name: _userDataCache[userId].Name,
                        telegramId: _userDataCache[userId].TelegramId,
                        direction: _userDataCache[userId].Direction));
            }
            else
            {
                await client.SendMessage(
                    chatId: userId,
                    text: "Некорректное направление. Используйте: 1C, WEB или Manager.");
            }
        }
    }
}