﻿using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class PublicHoliday
{
    public DateTime Date { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int HolidayId { get; set; }
}
