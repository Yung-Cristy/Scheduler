using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public abstract class Page
    {
        public abstract string Text { get; } 
        public abstract InlineKeyboardMarkup Keyboard { get; } 

        public async Task SendAsync(ITelegramBotClient client, long chatId)
        {
            await client.SendMessage(
                chatId: chatId,
                text: Text,
                replyMarkup: Keyboard
            );      
        }
        public async Task UpdateAsync(ITelegramBotClient client, long chatId, int messageId)
        {
            await client.DeleteMessage(chatId, messageId);

            await client.SendMessage(
                chatId: chatId,
                text: Text,
                replyMarkup: Keyboard
            );
        }

        public async Task EditAsync(ITelegramBotClient client, long chatId, int messageId)
        {
            await client.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: Text,
                replyMarkup: Keyboard
            );
        }


    }
}