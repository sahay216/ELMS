using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class CompanyProfileView
    {
        public int CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public DateOnly? DateOfEstablishment { get; set; }
        public string? Industry { get; set; }
        public int? NumberOfEmployees { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? Website { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

    }
}
