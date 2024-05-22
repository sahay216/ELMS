using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class AttendanceRecord
{
    public int RecordId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public DateOnly? Date { get; set; }

    public double? TotalHours { get; set; }

    public virtual UserDetail? Employee { get; set; }
}
