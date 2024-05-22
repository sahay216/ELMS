using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class EmailNotification
{
    public int NotificationId { get; set; }

    public string? LeaveType { get; set; }

    public int? ReferenceId { get; set; }

    public string? Status { get; set; }

    public DateTime? SentTime { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? RecipientEmail { get; set; }
}
