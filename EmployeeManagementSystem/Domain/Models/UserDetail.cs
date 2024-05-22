using System;
using System.Collections.Generic;

namespace Domain.Models;

public partial class UserDetail
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public int? RoleId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? ProfilePicture { get; set; }

    public string? SocialMedias { get; set; }

    public string? LanguagePreference { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string? UserRole { get; set; }

    public string? SecurityQuestion { get; set; }

    public string? SecurityAnswer { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CompanyName { get; set; }

    public bool IsDeleted { get; set; }

    public int? CompanyId { get; set; }

    public virtual ICollection<ApplicationMessage> ApplicationMessageReceivers { get; set; } = new List<ApplicationMessage>();

    public virtual ICollection<ApplicationMessage> ApplicationMessageSenders { get; set; } = new List<ApplicationMessage>();

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<LeaveApplication> LeaveApplicationEmployees { get; set; } = new List<LeaveApplication>();

    public virtual ICollection<LeaveApplication> LeaveApplicationManagers { get; set; } = new List<LeaveApplication>();

    public virtual Role? Role { get; set; }
}
