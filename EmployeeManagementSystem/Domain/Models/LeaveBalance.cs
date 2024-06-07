using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class LeaveBalance
{
    public int? EmployeeId { get; set; }

    public int? TotalEntitled { get; set; }

    public int? UsedLeave { get; set; }

    public int? RemainingLeaves { get; set; }

    public int? LeaveTypeId { get; set; }

    public int BalanceId { get; set; }

    public virtual UserDetail? Employee { get; set; }

    public virtual TypesOfLeave? LeaveType { get; set; }
}
