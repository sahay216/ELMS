using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class LoginView
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is Required")]
        public string Password { get; set; }

    }
}
