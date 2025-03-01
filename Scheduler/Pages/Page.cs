using Scheduler.User;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public abstract class Page
    {
        public abstract string Text { get; } 
        public abstract InlineKeyboardMarkup Keyboard { get; } 

        public async Task SendAsync(ITelegramBotClient client, long chatId, UserData userData)
        {
            var sentMessage = await client.SendMessage(
                chatId: chatId,
                text: Text,
                replyMarkup: Keyboard
            );

            userData.LastBotMessageId = sentMessage.MessageId;
        }
        public async Task UpdateAsync(ITelegramBotClient client, long chatId, int messageId, UserData userData)
        {
            await client.DeleteMessage(chatId, messageId);

            var sentMessage = await client.SendMessage(
                chatId: chatId,
                text: Text,
                replyMarkup: Keyboard
            );

            userData.LastBotMessageId = sentMessage.MessageId;
        }

        public async Task EditAsync(ITelegramBotClient client, long chatId, int messageId, UserData userData)
        {
            try
            {
                if (Keyboard != null)
                {
                    await client.EditMessageText(
                        chatId: chatId,
                        messageId: userData.LastBotMessageId, 
                        text: Text,
                        replyMarkup: Keyboard
                    );
                }
                else
                {
                    await client.EditMessageText(
                        chatId: chatId,
                        messageId: userData.LastBotMessageId, 
                        text: Text
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании сообщения: {ex.Message}");

                await UpdateAsync(
                    client:client,
                    chatId: chatId,
                    messageId: userData.LastBotMessageId,
                    userData: userData);
            }
        }
    }
}