using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public class ChangeEmployeesPage : Page
    {
        public override string Text => "Что требуется сделать со списком сотрудников?";
        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
    [
        [
            InlineKeyboardButton.WithCallbackData("Добавить сотрудника"),
            InlineKeyboardButton.WithCallbackData("Удалить сотрудника")
        ]
    ]);

    }
}
