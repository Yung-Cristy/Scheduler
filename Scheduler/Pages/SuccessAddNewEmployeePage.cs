using Scheduler.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public class SuccessAddNewEmployeePage : Page
    {
        private readonly string _name;
        private readonly long _telegramId;
        private readonly Direction _direction;

        public SuccessAddNewEmployeePage(string name, long telegramId, Direction direction)
        {
            _name = name;
            _telegramId = telegramId;
            _direction = direction;
        }

        public override string Text => $"Новый сотрудник успешно добавлен:\n\n" +
                                      $"Имя: {_name}\n" +
                                      $"Telegram ID: {_telegramId}\n" +
                                      $"Направление: {_direction}";

        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Вернуться в главное меню")
        );
    }
}
