﻿using System;
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
                    page: new RequestNameOfDeletingEmployee(),
                    userData: userData);
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

        if (text == "/start")
        {
            await _userStateManager.ShowPageAsync(
                userId: userId,
                page: new MainMenu(),
                userData: userData);

            await DeleteUserMessage(client, chatId, messageId);
        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestNameOfNewEmployeePage)
        {
            userData.Name = text;

            await DeleteUserMessage(client, chatId, messageId);

            await _userStateManager.EditPageAsync(
                userId: userId,
                messageId: userData.LastBotMessageId,
                page: new RequestTelegramIdOfNewEmployeePage(),
                userData: userData);
        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestTelegramIdOfNewEmployeePage)
        {
            userData.TelegramId = long.Parse(text);

            await _userStateManager.EditPageAsync(
                userId: userId,
                messageId: messageId,
                page: new RequestDirectionOfNewEmployeePage(),
                userData: userData);

            await DeleteUserMessage(client, chatId, messageId);
        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestDirectionOfNewEmployeePage)
        {
            if (Enum.TryParse(text, out Direction direction))
            {
                userData.Direction = direction;

                _employeesManager.AddEmployee(
                    employee: new Employee(
                        name: userData.Name,
                        telegramId: userData.TelegramId,
                        direction: userData.Direction));

                await DeleteUserMessage(client, chatId, messageId);

                await _userStateManager.UpdatePageAsync(
                    userId: userId,
                    messageId: userData.LastBotMessageId, 
                    page: new SuccessAddNewEmployeePage(
                        name: userData.Name,
                        telegramId: userData.TelegramId,
                        direction: userData.Direction),
                        userData: userData);
            }
            else
            {
                await client.SendMessage(
                    chatId: userId,
                    text: "Некорректное направление. Используйте: 1C, WEB или Manager.");
            }
        }
        else if (_userStateManager.GetCurrentPage(userId) is RequestNameOfDeletingEmployee)
        {
            userData.TelegramId = long.Parse(text);

            _employeesManager.RemoveEmployee(
                client: client,
                chatId: chatId,
                telegramId: userData.TelegramId);

            await _userStateManager.EditPageAsync(
                userId: userId,
                messageId: messageId,
                page: new SuccessDeleteEmployeePage(telegramId: userData.TelegramId),
                userData: userData);

            await DeleteUserMessage(client, chatId, messageId);
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