using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class EmployeeDetail
{
    public int EmployeeId { get; set; }

    public string? Department { get; set; }

    public int ManagerId { get; set; }

    public string? ManagerName { get; set; }

    public virtual UserDetail Employee { get; set; } = null!;

    public virtual UserDetail Manager { get; set; } = null!;
}
