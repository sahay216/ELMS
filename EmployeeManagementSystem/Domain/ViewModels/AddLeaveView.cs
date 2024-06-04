using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class AddLeaveView
    {
        public string LeaveName { get; set; } = null!;

        public string LeaveDescription { get; set; }

        public int LeaveQuota { get; set; }
    }
}
