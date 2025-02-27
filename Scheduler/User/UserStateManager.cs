using Scheduler.Pages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Scheduler.User
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

        public async Task ShowPageAsync(long userId, Page page)
        {
            _usersStateStorage[userId] = page; 
            await page.SendAsync(_client, userId);
        }

        public async Task UpdatePageAsync(long userId, int messageId, Page page)
        {
            _usersStateStorage[userId] = page; 
            await page.UpdateAsync(_client, userId, messageId);
        }

        public async Task EditPageAsync(long userId, int messageId, Page page)
        {
            _usersStateStorage[userId] = page;
            await page.EditAsync(_client, userId, messageId);
        }



        public Page GetCurrentPage(long userId)
        {
            return _usersStateStorage.TryGetValue(userId,out var page) ? page : null;
        }
    }
}
