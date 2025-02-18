using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace StudyXLS.Pages
{
    public class ChangeSchedulePage : Page
    {
        public override string Text => "Пожалуйста, загрузите заполненный шаблон в формате XLS (ссылка на пустой шаблон)";

        public override ReplyKeyboardMarkup Keyboard => new(new[]
        {
            new[] { new KeyboardButton("Загрузить расписание")},
            new [] {new KeyboardButton("Удалить")}
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }
}
