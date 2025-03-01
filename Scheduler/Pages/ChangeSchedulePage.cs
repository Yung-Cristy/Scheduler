using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public class ChangeSchedulePage : Page
    {
        public override string Text => "Что требуется сделать с расписанием?";

        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
    [
        [
            InlineKeyboardButton.WithCallbackData("Загрузить расписание"),
            InlineKeyboardButton.WithCallbackData("Удалить расписание")
        ]
    ]);
    }
}
