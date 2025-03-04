using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public class MainMenu : Page
    {
        public override string Text => " Чем могу тебе помочь?";

        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
            [
                [
                    InlineKeyboardButton.WithCallbackData("Изменить расписание"),
                    InlineKeyboardButton.WithCallbackData("Отсутствие")
                ],
                [
                    InlineKeyboardButton.WithCallbackData("Расписание на сегодня"),
                    InlineKeyboardButton.WithCallbackData("Изменить список сотрудников")
                ]
            ]);
    }          
}

