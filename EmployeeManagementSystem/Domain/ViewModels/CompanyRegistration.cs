using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class CompanyRegistration
    {
        public string? CompanyName { get; set; }

        public DateOnly? DateOfEstablishment { get; set; }

        public string? Industry { get; set; }

        public string? Location { get; set; }

        public string? Country { get; set; }

        public string? Website { get; set; }

        public string? Email { get; set; }
        public string? DomainName { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var results = new List<ValidationResult>();
            if (CompanyName == null || !Regex.Match(CompanyName, "^[a-zA-Z'-]+$").Success) results.Add(new ValidationResult("Name can only have the English alphabet, hyphens, and apostrophes", [nameof(CompanyName)]));

            if (Industry == null ) results.Add(new ValidationResult("Enter a valid Industry", [nameof(Industry)]));

            if (Email == null || !Regex.Match(Email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$").Success) results.Add(new ValidationResult("Invalid email address", [nameof(Email)]));

            if (Password == null || !Regex.Match(Password, "^.{4,}$").Success) results.Add(new ValidationResult("Password should be atleast 4 characters long", [nameof(Password)]));

            if (Phone == null || !Regex.Match(Phone, "^\\d{10}(?:\\d{3})?$").Success) results.Add(new ValidationResult("Phone number should be 10 digits long in case of domestic phone number, or 13 digits without '+' sign in case of international phone number", [nameof(Phone)]));

            if (DomainName == null || DomainName != Email.Split('@')[1]) results.Add(new ValidationResult("Please select a valid Security Question", [nameof(DomainName)]));
            if (Website == null) results.Add(new ValidationResult("Please Enter your Answer!", [nameof(Website)]));
            return results;

        }
    }
}
