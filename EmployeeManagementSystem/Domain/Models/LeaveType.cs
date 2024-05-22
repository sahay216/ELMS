using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class LeaveType
{
    public int LeaveTypeId { get; set; }

    public string? LeaveTypeName { get; set; }

    public int? AllotedLeaves { get; set; }

    public bool? CarryOverPolicy { get; set; }

    public int? CompensatoryOff { get; set; }

    public int? CompanyId { get; set; }

    public bool? IsGlobal { get; set; }

    public virtual Company? Company { get; set; }
}
