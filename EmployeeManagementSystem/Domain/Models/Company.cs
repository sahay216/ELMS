using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public DateOnly? DateOfEstablishment { get; set; }

    public string? Industry { get; set; }

    public int? NumberOfEmployees { get; set; }

    public string? Location { get; set; }

    public string? Country { get; set; }

    public string? Website { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? DomainName { get; set; }

    public string? PasswordSalt { get; set; }

    public string? PasswordHash { get; set; }

    public virtual ICollection<CompanyLeaves> CompanyLeaves { get; set; } = new List<CompanyLeaves>();

    public virtual ICollection<UserDetail> UserDetails { get; set; } = new List<UserDetail>();
}
