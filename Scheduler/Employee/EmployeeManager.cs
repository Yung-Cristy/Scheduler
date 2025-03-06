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

        public void RemoveEmployee(ITelegramBotClient client, long chatId, string name)
        {
            var employee = _employees.FirstOrDefault(e => e.Name == name);
            _employees.Remove(employee);
            SaveEmployees();
        }

        public  bool IsContain (string name)
        {
             return _employees.FirstOrDefault(x => x.Name == name) is not null;

        }

        public List<Employee> GetAllEmployees()
        {
            return _employees;
        }

        public bool TryGetEmployee(string name, out Employee employee)
        {
            employee = _employees.FirstOrDefault(e => e.Name == name);
            return employee is null;
        }
    }
}
