using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    class RequestNameOfNewEmployeePage : Page
    {
        public override string Text => "Введите ФИО нового сотрудника";

        public override InlineKeyboardMarkup Keyboard => null;
    }
}
