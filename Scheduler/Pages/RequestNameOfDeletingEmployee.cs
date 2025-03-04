using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    class RequestNameOfDeletingEmployee : Page
    {
        public override string Text => "Введите Telegram ID сотрудника, которого необходимо удалить";

        public override InlineKeyboardMarkup Keyboard => null;
            
        
    }
}
