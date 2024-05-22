
using Domain.Models;
using Domain.ViewModels;
using Services.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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

        public int? GetRoleId (string roleName)
        {
            return _trackerContext.Roles.FirstOrDefault(r => r.RoleName == roleName)?.RoleId;
        }

        public string? GetRoleName (string userEmail)
        {
            int? roleId = _trackerContext.UserDetails.FirstOrDefault(u => u.Email == userEmail).RoleId;
            return _trackerContext.Roles.FirstOrDefault(r=>r.RoleId== roleId)?.RoleName;
        }
        public async Task<bool> StoreUser (RegistrationView user, string passwordhash, string passwordsalt)
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
                    ProfilePicture = profilePicture

                    
                };
                _trackerContext.UserDetails.Add(userinfo);//added user to context 
                _trackerContext.SaveChanges();
                return true;
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

        public (bool loginSuccess, bool isfirstTimeLogin) CheckUser (LoginView user)
        {
            UserDetail userDetails = _trackerContext.UserDetails.FirstOrDefault(u => u.Email == user.Email);
            if (userDetails != null)
            {
                if ( _loginAuthentication.AuthLogin(user.Password, userDetails.PasswordSalt, userDetails.PasswordHash)){

                    bool isfirstTimeLogin = CheckFirstTimeLogin(user);
                    if (!isfirstTimeLogin)
                    {
                        userDetails.LastLoginDate = DateTime.Now;
                        _trackerContext.SaveChanges();
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
        public string CheckCompany(LoginView company)
        {
            Company companyDetails = _trackerContext.Company.FirstOrDefault(c => c.Email == company.Email);
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
                return "Company not found.";
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
            
            return _trackerContext.UserDetails.Where(u => !u.IsDeleted &&
                                                     (u.FirstName.Contains(query) || u.LastName.Contains(query)))
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
    }
}
