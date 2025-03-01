using Scheduler.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.User
{
    public  class UserData
    {
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public Direction Direction { get; set; }
        public int LastBotMessageId { get; set; }
    }
}
