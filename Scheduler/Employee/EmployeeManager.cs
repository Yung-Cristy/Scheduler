using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Scheduler.Employee
{
    class EmployeeManager
    {
        private List<Employee> _employees = new List<Employee>();

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public void RemoveEmployee (String name, ITelegramBotClient client,long chatId)
        {
            var deletingEmployee = _employees.FirstOrDefault(x => x.Name == name);

            if (deletingEmployee is null)
            {
                client.SendMessage(
                    chatId: chatId,
                    text: "Данный пользователь не найден");
            }
            else
            {
                _employees.Remove(deletingEmployee);
            }
        }
    }
}
