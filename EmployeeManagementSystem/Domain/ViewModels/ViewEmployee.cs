using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ViewEmployee
    {
        public int employeeId {  get; set; }
        public string? employeeName { get; set; }
        public string? employeeEmail { get; set; }
        public string? employeePhoneNumber { get; set;}
        public string? employeeAddress { get; set; }
        public string? employeeRole { get; set; }
        public string? employeeGender { get; set; }
        public DateTime? employeeCheckInTime { get; set; }
        public DateTime? employeeCheckOutTime { get; set; }
        public DateOnly employeeDOB { get; set; }

    }
}
