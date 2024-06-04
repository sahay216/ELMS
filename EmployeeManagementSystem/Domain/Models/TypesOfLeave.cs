using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class TypesOfLeave
{
    public int LeaveTypeId { get; set; }

    public string LeaveName { get; set; } = null!;

    public bool IsGlobal { get; set; }

    public int DefaultDays { get; set; }
}
