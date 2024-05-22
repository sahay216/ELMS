using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class ForgotPasswordView
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }
}