using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class CompanyLeaves
{
    public int? CompanyId { get; set; }

    public int? LeaveId { get; set; }

    public int LeaveQuota { get; set; }

    public string? LeaveDescription { get; set; }

    public string? LeaveName { get; set; }

    public int CompanyLeavesId { get; set; }

    public virtual Company? Company { get; set; }
}
