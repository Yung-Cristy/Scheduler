using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XSSF.UserModel;
using Telegram.Bot;
using Scheduler.Employee;

namespace Scheduler.Schedule
{
    class ScheduleTemplate
    {
        private XSSFWorkbook _template { get; set; }

        private readonly EmployeeManager _employees;

        public ScheduleTemplate(EmployeeManager employees)
        {
            _employees = employees;
            _template = new XSSFWorkbook();
        }

        private void Create()
        {
            
        }
    }
}
