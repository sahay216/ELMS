using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class ApplicationMessage
{
    public int MessageId { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? SentTimeDate { get; set; }

    public virtual UserDetail? Receiver { get; set; }

    public virtual UserDetail? Sender { get; set; }
}
