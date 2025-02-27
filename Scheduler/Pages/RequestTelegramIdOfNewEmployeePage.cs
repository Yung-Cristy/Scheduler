using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    class RequestTelegramIdOfNewEmployeePage : Page
    {
        public override string Text => "Введите telegram_id нового сотрудника";

        public override InlineKeyboardMarkup Keyboard => null;
    }
}
