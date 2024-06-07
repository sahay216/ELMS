using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class UserProfileView
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;


        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? ProfilePicture { get; set; }

        public string? SocialMedias { get; set; }

        public string? LanguagePreference { get; set; }


        public string? UserRole { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CompanyName { get; set; }

    }
}
