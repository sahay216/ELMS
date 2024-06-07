using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.ViewModels
{
    public class RegistrationView
    {
        public int EmployeeID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Department { get; set; }
        public string? ManagerName { get; set; }
        public int ManagerID { get; set; }

        public List<LeaveBalanceView> AddLeaveViews { get; set; } = new List<LeaveBalanceView>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            var birthdate = DateOnly.FromDateTime(DateOfBirth);
            var results = new List<ValidationResult>();
            if (FirstName == null || !Regex.Match(FirstName, "^[a-zA-Z'-]+$").Success) results.Add(new ValidationResult("Name can only have the English alphabet, hyphens, and apostrophes", [nameof(FirstName)]));
            
            if (LastName == null || !Regex.Match(LastName, "^[a-zA-Z'-]+$").Success) results.Add(new ValidationResult("Name can only have the English alphabet, hyphens, and apostrophes", [nameof(LastName)]));
            
            if (Email == null || !Regex.Match(Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) results.Add(new ValidationResult("Invalid email adress", [nameof(Email)]));
            
            if (Password == null || !Regex.Match(Password, "^.{4,}$").Success) results.Add(new ValidationResult("Password should be atleast 4 characters long", [nameof(Password)]));
            
            if (PhoneNumber == null || !Regex.Match(PhoneNumber, "^\\d{10}(?:\\d{3})?$").Success) results.Add(new ValidationResult("PhoneNumber number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number", [nameof(PhoneNumber)]));
            
            if ( birthdate.CompareTo(dateNow) > -1) results.Add(new ValidationResult("Date of birth should be a valid date", [nameof(DateOfBirth)]));
            else if (birthdate.AddYears(122).CompareTo(dateNow) < 0) results.Add(new ValidationResult("You are not that old, input a valid date", [nameof(DateOfBirth)]));
            else if (birthdate.AddYears(18).CompareTo(dateNow) > -1) results.Add(new ValidationResult("You must be atleast 18 years old to register", [nameof(DateOfBirth)]));
            if (Role == null || !Regex.Match(Role, "^[a-zA-Z'-]+$").Success) results.Add(new ValidationResult("Name can only have the English alphabet, hyphens, and apostrophes", [nameof(Role)]));
            if (Department == null ) results.Add(new ValidationResult("Enter correct Department", [nameof(Department)]));
            if (ManagerName == null || !Regex.Match(ManagerName, "^[a-zA-Z'\\-\\s]+$").Success) results.Add(new ValidationResult("Please Enter a valid Manager Name", [nameof(ManagerName)]));


            return results;

        }
    }

    public class LeaveBalanceView
    {
        public int? LeaveTypeID { get; set; }
        public string? LeaveName { get; set; }
        public int AllotedDays { get; set; }
    }
}
