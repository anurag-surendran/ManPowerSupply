using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Models
{
    public class DashboardAttendanceModel
    {
        public DateTime Date { get; set; }

        public int AttendanceCount { get; set; }

        public int Amount { get; set; }
    }
}
