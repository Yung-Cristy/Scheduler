using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace StudyXLS.Pages
{
    public abstract class Page
    {
        public abstract string Text { get; } 
        public abstract ReplyKeyboardMarkup Keyboard { get; } 

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
    }
}