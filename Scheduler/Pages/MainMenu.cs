using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace StudyXLS.Pages
{
    public class MainMenu : Page
    {
        public override string Text => "Привет, меня зовут Шедулер. Чем могу тебе помочь?";

        public override ReplyKeyboardMarkup Keyboard => new(new[]
        {
            new[] { new KeyboardButton("Изменить расписание"), new KeyboardButton("Отсутствие") },
            new [] {new KeyboardButton("Расписание на сегодня"), new KeyboardButton("Изменить список сотрудников") }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }          
}

