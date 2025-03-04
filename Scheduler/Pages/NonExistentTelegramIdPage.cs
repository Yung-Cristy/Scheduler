using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    class NonExistentTelegramIdPage : Page
    {
        public override string Text => "Сотрудника с указанным Telegram ID не существует";
        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Вернуться в главное меню")
        );
    }
}
