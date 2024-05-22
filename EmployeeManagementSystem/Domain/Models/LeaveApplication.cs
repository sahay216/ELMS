using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class LeaveApplication
{
    public int LeaveId { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? LeaveTypeId { get; set; }

    public string ReasonDescription { get; set; } = null!;

    public string? ApplicationStatus { get; set; }

    public DateTime? AppliedOn { get; set; }

    public int? ManagerId { get; set; }

    public virtual UserDetail? Employee { get; set; }

    public virtual UserDetail? Manager { get; set; }
}
