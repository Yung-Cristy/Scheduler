using StudyXLS.Pages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace StudyXLS.User
{
    public class UserStateManager
    {
        private readonly ITelegramBotClient _client;
        private readonly ConcurrentDictionary<long, Page> _usersStateStorage;

        public UserStateManager(TelegramBotClient client)
        {
            _client = client;
            _usersStateStorage = new ConcurrentDictionary<long, Page>();
        }

        public async Task ShowPageAsync(long chatId, Page page)
        {
            _usersStateStorage[chatId] = page; 
            await page.SendAsync(_client, chatId);
        }

        public async Task UpdatePageAsync(long chatId, int messageId, Page page)
        {
            _usersStateStorage[chatId] = page; 
            await page.UpdateAsync(_client, chatId, messageId);
        }       
    }
}
