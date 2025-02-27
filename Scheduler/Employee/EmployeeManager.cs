using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Scheduler.Employee
{
    public class EmployeeManager
    {
        private List<Employee> _employees = new List<Employee>();
        private readonly string _filePath = "Employees.json";

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
            SaveEmployees();
        }

        public EmployeeManager()
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    _employees = new List<Employee>();
                }
                else
                {
                    _employees = JsonSerializer.Deserialize<List<Employee>>(json);
                }
            }
            else
            {
                _employees = new List<Employee>();
                SaveEmployees();
            }
        }

        private void SaveEmployees ()
        {
            var json = JsonSerializer.Serialize(_employees, new JsonSerializerOptions { WriteIndented =true});
            File.WriteAllText(_filePath, json);
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
                SaveEmployees();
            }
        }

        public  Employee GetEmployee (string name)
        {
            return _employees.First(x => x.Name.ToLower() == name.ToLower());
        }
        
        public List<Employee> GetAllEmployees()
        {
            return _employees;
        }
    }
}
