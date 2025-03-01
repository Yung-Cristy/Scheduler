using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Scheduler.Schedule;

namespace Scheduler.Employee
{
    public class Employee
    {
        public string Name { get; set; }
        public long TelegramId { get; set; }
        public Direction Direction { get; }
        public List<Duty> Duties { get; set; }
        public bool IsDutyEmployee { get; set; }

        public Employee()
        {
            Duties = new List<Duty>(); 
            IsDutyEmployee = false; 
        }

        
        public Employee(string name, long telegramId, Direction direction) : this()    
        {
            Name = name;
            TelegramId = telegramId;
            Direction = direction;
        }



        public void ToggleDuty (Employee dutyEmployeeNow)
        {
            dutyEmployeeNow.IsDutyEmployee = !IsDutyEmployee;
        }
    }
}
