using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public partial class EmployeeTrackerContext : DbContext
{
    public EmployeeTrackerContext()
    {
    }

    public EmployeeTrackerContext(DbContextOptions<EmployeeTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationMessage> ApplicationMessages { get; set; }

    public virtual DbSet<AttendanceRecord> AttendanceRecords { get; set; }

    public virtual DbSet<Company> Company { get; set; }

    public virtual DbSet<CompanyLeaves> CompanyLeaves { get; set; }

    public virtual DbSet<EmailNotification> EmailNotifications { get; set; }

    public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public virtual DbSet<LeaveApplication> LeaveApplications { get; set; }

    public virtual DbSet<LeaveBalance> LeaveBalances { get; set; }

    public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TypesOfLeave> TypesOfLeaves { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EmployeeTracker;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Applicat__C87C037CD86DB3BD");

            entity.Property(e => e.MessageId)
                .ValueGeneratedNever()
                .HasColumnName("MessageID");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SentTimeDate).HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.ApplicationMessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK__Applicati__Recei__17036CC0");

            entity.HasOne(d => d.Sender).WithMany(p => p.ApplicationMessageSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Applicati__Sende__160F4887");
        });

        modelBuilder.Entity<AttendanceRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__Attendan__FBDF78C96D7A0D6E");

            entity.ToTable("Attendance_Records");

            entity.Property(e => e.RecordId)
                .ValueGeneratedNever()
                .HasColumnName("RecordID");
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.TotalHours).HasColumnName("Total_Hours");

            entity.HasOne(d => d.Employee).WithMany(p => p.AttendanceRecords)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Attendanc__Emplo__0F624AF8");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971C4C3609FFEE");

            entity.ToTable("Company");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.DomainName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Industry).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        modelBuilder.Entity<CompanyLeaves>(entity =>
        {
            entity.HasKey(e => e.CompanyLeavesId).HasName("PK__CompanyL__6F2D6FADD9FD5E52");

            entity.HasIndex(e => new { e.CompanyId, e.LeaveId }, "UQ__CompanyL__AA01C7DA88B7F3DC").IsUnique();

            entity.Property(e => e.CompanyLeavesId).HasColumnName("CompanyLeavesID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.LeaveDescription).HasMaxLength(255);
            entity.Property(e => e.LeaveId).HasColumnName("LeaveID");
            entity.Property(e => e.LeaveName).HasMaxLength(255);

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyLeaves)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__CompanyLe__Compa__40F9A68C");
        });

        modelBuilder.Entity<EmailNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__EmailNot__20CF2E327196D68A");

            entity.ToTable("EmailNotification");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");
            entity.Property(e => e.LeaveType).HasMaxLength(50);
            entity.Property(e => e.RecipientEmail).HasMaxLength(150);
            entity.Property(e => e.ReferenceId).HasColumnName("ReferenceID");
            entity.Property(e => e.SentTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Subject).HasMaxLength(255);
        });

        modelBuilder.Entity<EmployeeDetail>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1F04E85A6");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeeDetailEmployee)
                .HasForeignKey<EmployeeDetail>(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeDetails_UserDetails");

            entity.HasOne(d => d.Manager).WithMany(p => p.EmployeeDetailManagers)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeDetails_Manager");
        });

        modelBuilder.Entity<LeaveApplication>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__Leave_Ap__796DB97965B7D9A1");

            entity.ToTable("Leave_Application");

            entity.Property(e => e.LeaveId)
                .ValueGeneratedNever()
                .HasColumnName("LeaveID");
            entity.Property(e => e.ApplicationStatus).HasMaxLength(50);
            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveApplicationEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Leave_App__Emplo__0B91BA14");

            entity.HasOne(d => d.Manager).WithMany(p => p.LeaveApplicationManagers)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Leave_App__Manag__0C85DE4D");
        });

        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__Leave_Ba__A760D59E19B4D789");

            entity.ToTable("Leave_Balance");

            entity.Property(e => e.BalanceId).HasColumnName("BalanceID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveBalances)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Leave_Bal__Emplo__0A9D95DB");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveBalances)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK__Leave_Bal__Leave__46B27FE2");
        });

        modelBuilder.Entity<PublicHoliday>(entity =>
        {
            entity.HasKey(e => e.HolidayId).HasName("PK__Public_H__2D35D59A17B4D321");

            entity.ToTable("Public_Holidays");

            entity.Property(e => e.HolidayId).HasColumnName("HolidayID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AF3BC3A41");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61606E30566A").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<TypesOfLeave>(entity =>
        {
            entity.HasKey(e => e.LeaveTypeId).HasName("PK__Types_of__43BE8FF4DC767EC3");

            entity.ToTable("Types_of_Leaves");

            entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");
            entity.Property(e => e.IsGlobal)
                .HasDefaultValue(true)
                .HasColumnName("isGlobal");
            entity.Property(e => e.LeaveName).HasMaxLength(125);
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserDeta__1788CCAC7E4C6BB8");

            entity.HasIndex(e => e.Email, "UQ__UserDeta__A9D10534E8DF078E").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LanguagePreference).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.SocialMedias).HasMaxLength(255);
            entity.Property(e => e.UserRole).HasMaxLength(50);

            entity.HasOne(d => d.Company).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__UserDetai__Compa__282DF8C2");

            entity.HasOne(d => d.Role).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserDetai__RoleI__07C12930");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
