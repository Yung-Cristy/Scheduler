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

        if (!_userDataCache.TryGetValue(userId, out var userData))
        {
            userData = new UserData();
            _userDataCache[userId] = userData;
        }

        switch (inputCallback)
        {
            case "Изменить расписание":
                await _userStateManager.EditPageAsync(
                    userId: userId,
                    messageId: messageId,
                    new ChangeSchedulePage(),
                    userData: userData);
                break;

            case "Изменить список сотрудников":
                await _userStateManager.EditPageAsync(
                    userId: userId,
                    messageId: messageId,
                    new ChangeEmployeesPage(),
                    userData: userData);
                break;

            case "Добавить сотрудника":
                await _userStateManager.EditPageAsync(
                    userId: userId,
                    messageId: messageId,
                    new RequestNameOfNewEmployeePage(),
                    userData: userData);
                break;

            case "Вернуться в главное меню":
                await _userStateManager.EditPageAsync(
                    messageId: messageId,
                    userId: userId,
                    page: new MainMenu(),
                    userData: userData);
                break;

            case "Удалить сотрудника":
                await _userStateManager.EditPageAsync(
                    messageId: messageId,
                    userId: userId,
                    page: new RequestDeletingEmployee(_employeesManager),
                    userData: userData);
                break;
            case "OneC":
            case "Web":
            case "Manager":
                if (Enum.TryParse(inputCallback, out Direction direction))
                {
                    userData.Direction = direction;

                    _employeesManager.AddEmployee(
                        employee: new Employee(
                            name: userData.Name,
                            telegramId: userData.TelegramId,
                            direction: userData.Direction));

                    await _userStateManager.EditPageAsync(
                        userId: userId,
                        messageId: messageId,
                        page: new SuccessAddNewEmployeePage(
                            name: userData.Name,
                            telegramId: userData.TelegramId,
                            direction: userData.Direction),
                        userData: userData);
                }
                break;
            default:
                if (_userStateManager.GetCurrentPage(userId) is RequestDeletingEmployee)
                {
                    if (_employeesManager.IsContain(inputCallback))
                    {
                        _employeesManager.RemoveEmployee(
                            client: client,
                            chatId: chatId,
                            name: inputCallback);

                        await _userStateManager.EditPageAsync(
                            userId: userId,
                            messageId: messageId,
                            page: new SuccessDeleteEmployeePage(inputCallback),
                            userData: userData);
                    }                    
                }
                else
                {
                    await client.SendMessage(
                        chatId: userId,
                        text: "Неизвестная команда.");
                }
                break;
        }
    }

    private static async Task HandleMessage(ITelegramBotClient client, Update update)
    {
        var chatId = update.Message.Chat.Id;
        var messageId = update.Message.MessageId;
        var text = update.Message.Text;
        var userId = update.Message.From.Id;

        if (!_userDataCache.TryGetValue(userId, out var userData))
        {
            userData = new UserData();
            _userDataCache[userId] = userData;
        }

        var currentPage = _userStateManager.GetCurrentPage(userId);

        if (text == "/start")
        {
            if (userData.LastBotMessageId != 0)
            {
                await _userStateManager.EditPageAsync(
                    userId: userId,
                    messageId: userData.LastBotMessageId,
                    page: new MainMenu(),
                    userData: userData);
            }
            else
            {
                await _userStateManager.ShowPageAsync(
                    userId: userId,
                    page: new MainMenu(),
                    userData: userData);
            }

            await DeleteUserMessage(client, chatId, messageId);
        }
        else if (currentPage is RequestNameOfNewEmployeePage)
        {
            userData.Name = text;

            await DeleteUserMessage(client, chatId, messageId);

            await _userStateManager.EditPageAsync(
                userId: userId,
                messageId: userData.LastBotMessageId,
                page: new RequestTelegramIdOfNewEmployeePage(),
                userData: userData);
        }
        else if (currentPage is RequestTelegramIdOfNewEmployeePage)
        {
            userData.TelegramId = long.Parse(text);
            await DeleteUserMessage(client, chatId, messageId);

            await _userStateManager.EditPageAsync(
                userId: userId,
                messageId: messageId,
                page: new RequestDirectionOfNewEmployeePage(),
                userData: userData);
        }
    }

    private static async Task DeleteUserMessage(ITelegramBotClient client, long chatId, int messageId)
    {
        try
        {
            await client.DeleteMessage(
                chatId: chatId,
                messageId: messageId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении сообщения: {ex.Message}");
        }
    }
}