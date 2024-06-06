using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class UserLeaveBalanceView
    {
        public int? TotalEntitled { get; set; }

        public int? UserID { get; set; }
        public int? Balance { get; set; }

        public string? LeaveName { get; set; }

        public int? LeaveTypeId { get; set; }

    }
}
