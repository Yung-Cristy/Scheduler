﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Schedule
{
    public class Duty
    {
        public DateTime Date { get; set; } 
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get;set; }
    }
}
