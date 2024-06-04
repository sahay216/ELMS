using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class LeaveReportView
    {
        public string? LeaveName { get; set; }
        public int AllotedDays { get; set; }
        public int? AvailableDays { get; set; }

    }
}
