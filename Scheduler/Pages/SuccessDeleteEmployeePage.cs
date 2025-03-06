using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    public class SuccessDeleteEmployeePage : Page
    {
        public override string Text => $"Сотрудник {_nameOfDeletedEmployee} успешно удален";
        private readonly string  _nameOfDeletedEmployee;

        public override InlineKeyboardMarkup Keyboard => new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("Вернуться в главное меню")
        );
        public SuccessDeleteEmployeePage(string name)
        {
            _nameOfDeletedEmployee = name;
        }
    }
}
