using Scheduler.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Scheduler.Pages
{
    class RequestDeletingEmployee : Page
    {
        public override string Text {get;}
        public List<Employee.Employee> Employees { get; }
        public override InlineKeyboardMarkup Keyboard { get; }

        public RequestDeletingEmployee(EmployeeManager employees)
        {
            Keyboard = GetKeyboardWithEmployees(employees);
            Employees = employees.GetAllEmployees();

            if (Employees.Count == 0)
            {
                Text = "Список сотрудников пуст";
            }
            else
            {
                Text = "Выбери сотрудника, которого необходимо удалить";
            }
        }

        public static InlineKeyboardMarkup GetKeyboardWithEmployees(EmployeeManager listEmployees)
        {
            var keyboard = new InlineKeyboardMarkup();
            keyboard.AddNewRow();

            var employees = listEmployees.GetAllEmployees();

            if (employees.Count == 0)
            {
                keyboard.AddButton("Вернуться в главное меню");
                return keyboard;
            }

            foreach (var employee in employees)
            {
                keyboard.AddButton(
                    text: $"{employee.Name}",
                    callbackData: $"{employee.Name}");
            }

            return keyboard;
        }
    }
}
