using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    class RequestDirectionOfNewEmployeePage : Page
    {
        public override string Text => "Укажите направление нового сотрудника";

        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData("1C","OneC"),
                    InlineKeyboardButton.WithCallbackData("WEB","Web"),
                    InlineKeyboardButton.WithCallbackData("Руководитель","Manager")
                ]
            ]);
    }
}
