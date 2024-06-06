using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ApplyLeaveView
    {
        public UserDetail UserDetail { get; set; }
        public List<UserLeaveBalanceView>? UserLeaveBalances { get; set; }
    }
}
