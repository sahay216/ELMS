
using Domain.Models;
using Domain.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services.Authentication;

using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DatabaseAccess
{
    public class DatabaseOperations
    {
        private readonly EmployeeTrackerContext _trackerContext;
        private readonly LoginAuthentication _loginAuthentication;

        public DatabaseOperations(EmployeeTrackerContext trackerContext, LoginAuthentication loginAuthentication )
        {
            _trackerContext = trackerContext;
            _loginAuthentication = loginAuthentication;
        }
        //ok
        public int? GetRoleId (string roleName)
        {
            return _trackerContext.Roles.FirstOrDefault(r => r.RoleName == roleName)?.RoleId;
        }
        //ok
        public string? GetRoleName (string userEmail)
        {
            int? roleId = _trackerContext.UserDetails.FirstOrDefault(u => u.Email == userEmail).RoleId;
            return _trackerContext.Roles.FirstOrDefault(r=>r.RoleId== roleId)?.RoleName;
        }
        public async Task<bool> StoreUser (RegistrationView user, string passwordhash, string passwordsalt, int companyid)
        {
            using (var transaction = await _trackerContext.Database.BeginTransactionAsync())
            {
                try
                {
                    bool userPresent = await _trackerContext.UserDetails.AnyAsync(e => e.Email == user.Email);
                    if (userPresent)
                    {
                        throw new InvalidOperationException("A user is already present with this Email-id. Try login or register with another Email");
                    }
                    else
                    {
                        int? roleId = GetRoleId(user.Role);
                        string emailDomain = user.Email.Split('@')[1].Split('.')[0];
                        string? profilePicture = await UploadProfilePic(user);
                        
                        UserDetail userinfo = new UserDetail
                        {
                            //  EmployeeId = user.EmployeeID,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Address = user.Address,
                            PhoneNumber = user.PhoneNumber,
                            DateOfBirth = DateOnly.FromDateTime(user.DateOfBirth),
                            Gender = user.Gender,
                            RoleId = roleId.Value,
                            PasswordHash = passwordhash,
                            PasswordSalt = passwordsalt,
                            CreatedAt = DateTime.Now,
                            CompanyName = emailDomain,
                            ProfilePicture = profilePicture,
                            CompanyId = companyid

                        };
                        _trackerContext.UserDetails.Add(userinfo);//added user to context 
                        await _trackerContext.SaveChangesAsync();


                        UserDetail? manager = null;
                        manager = await _trackerContext.UserDetails.FirstOrDefaultAsync(u => u.UserId == user.ManagerID);

                        string managerFullname = $"{manager.FirstName} {manager.LastName}".Trim();
                        string enteredName = user.ManagerName.Trim();
                        if (manager == null || managerFullname != enteredName)
                        {
                            throw new InvalidOperationException("Manager Id and Name do not match.");
                        }
                        EmployeeDetail employeeDetail = new EmployeeDetail
                        {
                            Department = user.Department,
                            ManagerName = user.ManagerName,
                            EmployeeId = userinfo.UserId,
                            ManagerId = user.ManagerID,

                        };
                        _trackerContext.EmployeeDetails.Add(employeeDetail);
                        await _trackerContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public int CountEmployee(Company company)
        {
            int totalEmployee = _trackerContext.UserDetails
                .Where(C => C.CompanyName == company.DomainName).Count();
            
            company.NumberOfEmployees = totalEmployee;
            _trackerContext.SaveChanges();
            return totalEmployee;
        }

        public async Task<bool> StoreCompany(CompanyRegistration company, string passwordhash, string passwordsalt)
        {
            bool companyPresent = await _trackerContext.Company.AnyAsync(e => e.Email == company.Email);
            if (companyPresent)
            {
                throw new InvalidOperationException("A company with the same email already exists. Please log in.");
            }
            else
            {
                string companyEmailDomain = company.Email.Split('@')[1].Split('.')[0];
                Company companyinfo = new Company
                {
                    CompanyName = company.CompanyName,
                    Email = company.Email,
                    DateOfEstablishment = company.DateOfEstablishment,
                    Industry = company.Industry,
                    Location = company.Location,
                    Address = company.Address,
                    Country = company.Country,
                    Website = company.Website,
                    Phone = company.Phone,
                    DomainName = companyEmailDomain,
                    PasswordHash = passwordhash,
                    PasswordSalt = passwordsalt
                };
                _trackerContext.Company.Add(companyinfo);
                _trackerContext.SaveChanges();
                return true;
            }

        }

        public async Task<(bool loginSuccess, bool isfirstTimeLogin)> CheckUser (LoginView user)
        {
            UserDetail? userDetails = await _trackerContext.UserDetails.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userDetails != null)
            {
                if ( _loginAuthentication.AuthLogin(user.Password, userDetails.PasswordSalt, userDetails.PasswordHash)){

                    bool isfirstTimeLogin = CheckFirstTimeLogin(user);
                    if (!isfirstTimeLogin)
                    {
                        userDetails.LastLoginDate = DateTime.Now;
                        await _trackerContext.SaveChangesAsync();
                    }
                    
                    return (true, isfirstTimeLogin);
                };

            }
            return (false, false);
        }
        public bool CheckFirstTimeLogin(LoginView user)
        {
            UserDetail userDetail = _trackerContext.UserDetails.FirstOrDefault(u=> u.Email== user.Email);
            if(userDetail != null)
            {
                if(userDetail.LastLoginDate == null)
                {
                    return true;
                }
            }
            return false;
        }
        public bool UpdatePassword(ForgotPasswordView forgotPassword, string passwordHash, string passwordSalt)
        {
            UserDetail userDetail = _trackerContext.UserDetails.FirstOrDefault(u=> u.Email ==  forgotPassword.Email);
            if (userDetail != null)
            {
                var user = GetUsersById(userDetail.UserId);
                if (user != null)
                {
                    userDetail.PasswordHash = passwordHash;
                    userDetail.PasswordSalt = passwordSalt;
                    userDetail.SecurityQuestion = forgotPassword.SecurityQuestion;
                    userDetail.SecurityAnswer = forgotPassword.SecurityAnswer;
                    userDetail.LastLoginDate = DateTime.Now;
                }
                _trackerContext.UserDetails.Update(user);
                _trackerContext.SaveChanges();
                return true;
            }
            throw new InvalidDataException("Invalid Email entered. Kindly enter a valid Email");
        }
        public async Task<string> CheckCompany(LoginView company)
        {
            Company? companyDetails = await _trackerContext.Company.FirstOrDefaultAsync(c => c.Email == company.Email);
            if(companyDetails != null)
            {
                try
                {
                    if (_loginAuthentication.AuthLogin(company.Password, companyDetails.PasswordSalt, companyDetails.PasswordHash))
                    {
                        return null;
                    }
                    else
                    {
                        return "Invalid User Credentials.";
                    }
                }
                catch(InvalidOperationException ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "Company not found. Check Email/Password and try again.";
            }
        }
        public  async Task<string?> UploadProfilePic(RegistrationView user)
        {
            if (user.ProfilePicture != null)
            {
                var uploadsFolder = @"C:\Users\Coditas\Desktop\Employee Leave Tracker\Project\EmployeeManagementSystem\EmployeeManagementSystem\wwwroot\profilePicture";
                var fileName = user.FirstName + "_" + user.LastName + user.ProfilePicture.FileName;
                var filePath = Path.Combine(uploadsFolder, fileName);
                // Save the image data as a file in the wwwroot/images folder
                if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
                {
                    using (var fileStream2 = new FileStream(filePath, FileMode.Create))
                    {
                        await user.ProfilePicture.CopyToAsync(fileStream2);
                    }
                }
                return fileName;
            }
            return null;
        }

        public List<ViewEmployee> GetEmployees( string companyEmail)
        {
           
            var companyDetails = _trackerContext.Company.FirstOrDefault(c=> c.Email == companyEmail);
            var companyDomain = companyDetails.DomainName;
            
            var employees = _trackerContext.UserDetails
                            .Where(u => u.CompanyName == companyDomain && !u.IsDeleted)
                            .Select(u => new ViewEmployee
                            {
                                employeeId = u.UserId,
                                employeeName = $"{u.FirstName} {u.LastName}",
                                employeeEmail = u.Email,
                                employeeDOB = u.DateOfBirth,
                                employeeAddress = u.Address,
                                employeeGender = u.Gender,
                                employeePhoneNumber = u.PhoneNumber
                                //employeeCheckInTime
                                //employeeCheckOutTime
                            }).ToList();
                    foreach (var employee in employees)
                    {
                        employee.employeeRole = GetRoleName(employee.employeeEmail);
                    }
                    
            return employees;
            
        }

        public UserDetail GetUsersById(int id)
        {
            var user = _trackerContext.UserDetails.FirstOrDefault(u=> u.UserId == id);
            return  user;
        }

        public void UpdateUser(UserDetail updatedUser)
        {
            var existingUser = GetUsersById(updatedUser.UserId);
            if (existingUser != null)
            {
                existingUser.UserRole = updatedUser.UserRole;
                existingUser.Address = updatedUser.Address;
                existingUser.Gender = updatedUser.Gender;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.Email = updatedUser.Email;
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.DateOfBirth = updatedUser.DateOfBirth;
                existingUser.UpdatedAt = DateTime.Now;
            }
            _trackerContext.UserDetails.Update(existingUser);
            _trackerContext.SaveChanges();
        }

        public bool deleteEmployee (UserDetail user)
        {
           if(user!=null)
            {
                user.IsDeleted = true;
                _trackerContext.Update(user);
                _trackerContext.SaveChanges();
                return true;

            }
            return false;
        }

        public List<ViewEmployee> SearchEmployee(string query)
        {
            var lowerquery = query.ToLower();
            return _trackerContext.UserDetails.Where(u => !u.IsDeleted &&
                                                     (u.FirstName.ToLower().Contains(lowerquery) || u.LastName.ToLower().Contains(lowerquery)))
                                                     .Select(u => new ViewEmployee
                                                     {
                                                         employeeId = u.UserId,
                                                         employeeName = $"{u.FirstName} {u.LastName}",
                                                         employeeEmail = u.Email,
                                                         employeeDOB = u.DateOfBirth,
                                                         employeeAddress = u.Address,
                                                         employeeGender = u.Gender,
                                                         employeeRole = u.UserRole,
                                                         employeePhoneNumber = u.PhoneNumber
                                                     }).ToList();
        }

        public List<UserProfile> UserProfile(string userEmail)
        {
            var users = _trackerContext.UserDetails.FirstOrDefault(u=> u.Email== userEmail);
            string userRole = GetRoleName(userEmail);

            var profile = _trackerContext.UserDetails
                            .Where(u => u.Email == userEmail && !u.IsDeleted)
                            .Select(u => new UserProfile
                            {
                                 UserId = u.UserId,
                                 ProfilePicture = u.ProfilePicture,
                                FullName = $"{u.FirstName} {u.LastName}",
                                Email = u.Email,
                                DateOfBirth = u.DateOfBirth,
                                Address = u.Address,
                                Gender = u.Gender,
                                PhoneNumber = u.PhoneNumber,
                                UserRole = userRole,
                                
                            }).ToList();
            return profile;
        }

        public List<UserDetailDashboard> GetBirthdayEmployees()
        {
            var today = DateTime.Today;
            var birthdayEmployee = (from user in _trackerContext.UserDetails
                                    join employeeDetail in _trackerContext.EmployeeDetails
                                    on user.UserId equals employeeDetail.EmployeeId
                                    where user.DateOfBirth.Month == today.Month && user.DateOfBirth.Day == today.Day
                                    select new UserDetailDashboard
                                    {
                                        EmployeeName = $"{user.FirstName} {user.LastName}",
                                        EmployeeId = user.UserId,
                                        EmployeePhone = user.PhoneNumber,
                                        EmployeeImg = user.ProfilePicture,
                                        EmployeeDepartment = employeeDetail.Department
                                    }).ToList();


            return birthdayEmployee;
        }
        public async Task<List<UserDetailDashboard>> GetNewHires()
        {
            var twoweeksAgo = DateTime.Now.AddDays(-14);
            var newhires = (from user in _trackerContext.UserDetails
                            join employeeDetail in _trackerContext.EmployeeDetails
                            on user.UserId equals employeeDetail.EmployeeId
                            where user.CreatedAt.HasValue && user.CreatedAt.Value >= twoweeksAgo && user.IsDeleted == false
                            select new UserDetailDashboard
                            {
                                EmployeeName = $"{user.FirstName} {user.LastName}",
                                EmployeeId = user.UserId,
                                EmployeePhone = user.PhoneNumber,
                                EmployeeImg = user.ProfilePicture,
                                EmployeeDepartment = employeeDetail.Department
                            }).ToList();
            return newhires;
        }

        public async Task<int?> GetUserID (string UserEmail)
        {
            var user =  await _trackerContext.UserDetails.FirstOrDefaultAsync(u => u.Email == UserEmail);
            return user?.UserId;
        }
        public async Task<int?> GetAdminID(string UserEmail)
        {
            var user = await _trackerContext.Company.FirstOrDefaultAsync(u => u.Email == UserEmail);
            return user?.CompanyId;
        }

        public async Task<AttendanceRecord> StoreAttendanceCheckin(int userID)
        {
            var attendance = new AttendanceRecord
            {
                EmployeeId = userID,
                CheckInTime = DateTime.Now,
                TotalHours = 0
            };
            _trackerContext.AttendanceRecords.Add(attendance);
            await _trackerContext.SaveChangesAsync();

            return attendance;

        }

        public async Task<AttendanceRecord> StoreAttendanceCheckout(int userID)
        {
            var attendance = await _trackerContext.AttendanceRecords.Where(a=>a.EmployeeId == userID && a.CheckOutTime == null ).FirstOrDefaultAsync();

            if(attendance != null)
            {
                attendance.CheckOutTime = DateTime.Now;
                attendance.TotalHours = CalculateHours(attendance.CheckInTime, attendance.CheckOutTime).TotalHours;
                _trackerContext.AttendanceRecords.Update(attendance);
                await _trackerContext.SaveChangesAsync();
            }
            return attendance;
        }

        private TimeSpan CalculateHours(DateTime? checkinTime, DateTime? checkoutTime)
        {
            if(checkinTime.HasValue && checkoutTime.HasValue)
            {
                return checkoutTime.Value - checkinTime.Value;
            }
            return TimeSpan.Zero;
        }



        public bool AddLeaves(AddLeaveView leaves, int companyID)
        {
            
            string leaveNamePart = leaves.LeaveName.Split(' ')[0];
            var LeaveType = _trackerContext.TypesOfLeaves.AsEnumerable().FirstOrDefault(l => l.LeaveName.StartsWith(leaveNamePart, StringComparison.OrdinalIgnoreCase  ));

            if (LeaveType == null)
            {
                TypesOfLeave newLeave = new TypesOfLeave
                {
                    LeaveName = leaves.LeaveName,
                    DefaultDays = 1,
                    IsGlobal = true
                };
                _trackerContext.TypesOfLeaves.Add(newLeave);
                _trackerContext.SaveChanges();

                LeaveType = _trackerContext.TypesOfLeaves.FirstOrDefault(l=> l.LeaveName == newLeave.LeaveName && l.IsGlobal == newLeave.IsGlobal && l.DefaultDays == newLeave.DefaultDays);
            }
            CompanyLeaves leave = new CompanyLeaves
            {
                CompanyId = companyID,
                LeaveId = LeaveType.LeaveTypeId,
                LeaveQuota = leaves.LeaveQuota,
                LeaveDescription = leaves.LeaveDescription,
                LeaveName = leaves.LeaveName
            };
            _trackerContext.CompanyLeaves.Add(leave);
            _trackerContext.SaveChanges();
            return true;
        }

        public async Task<List<TypesOfLeave>> ViewLeaves(int companyID)
        {
            var leaves = await _trackerContext.TypesOfLeaves.ToListAsync();
            var companyLeaveDetails = await _trackerContext.CompanyLeaves
                .Where(c => c.CompanyId == companyID).ToListAsync();
                
            foreach(var leave in leaves)
            {
                var companyleave = companyLeaveDetails.FirstOrDefault(c => c.LeaveId == leave.LeaveTypeId);
                if (companyleave != null)
                {
                    leave.DefaultDays = companyleave.LeaveQuota;
                    leave.LeaveName = companyleave.LeaveName;
                }
                
            }
            return leaves;
        }

        public bool UpdateLeaves(int leaveID, int days)
        {
            var companyleave = _trackerContext.CompanyLeaves.FirstOrDefault(cl => cl.LeaveId == leaveID);
            if(companyleave == null)
            {
                return false;
            }
            companyleave.LeaveQuota = days;
            _trackerContext.SaveChanges();
            return true;
        }

        public async Task<List<LeaveReportView>> GetLeaveReport(int companyid)
        {
            var companyLeave = await _trackerContext.CompanyLeaves.Where(cl => cl.CompanyId == companyid).ToListAsync();
            var leaves = companyLeave.Select(cl => new LeaveReportView
            {
                LeaveName = cl.LeaveName,
                AllotedDays = cl.LeaveQuota
            }).ToList();
            return leaves;
        }
    }
}
