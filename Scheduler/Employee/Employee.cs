using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Schedule;

namespace Scheduler.Employee
{
    public enum Direction
    {
        OneC,    
        Web,     
        Manager  
    }
    public class Employee
    {
        public string Name { get; set; }
        public readonly long TelegramId;
        public Direction Direction { get; }
        public List<Duty> Duties { get; set; }
        public bool IsDutyEmployee { get; set; }

        public Employee(string name, long telegramId,Direction direction)
        {
            Name = name;
            TelegramId = telegramId;
            Direction = direction;
            IsDutyEmployee = false;
        }

        public void ToggleDuty (Employee dutyEmployeeNow)
        {
            dutyEmployeeNow.IsDutyEmployee = !IsDutyEmployee;
        }
    }
}
