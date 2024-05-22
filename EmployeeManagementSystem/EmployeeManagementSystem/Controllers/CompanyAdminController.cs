using EmployeeManagementSystem.Action_Atributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatabaseAccess;
using Domain.Models;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    public class CompanyAdminController : Controller
    {
        private readonly DatabaseOperations _databaseOperations;
        public CompanyAdminController(DatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
        }
        
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [NoCache]
        public IActionResult AdminPage()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult NotAdmin()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
                
        }
        [HttpGet]
        public IActionResult EmployeeRegistration()
        {
            return RedirectToAction("RegisterEmployee", "Login");
        }

        [HttpGet]
        public IActionResult ViewEmployees()
        {
            if (User.Identity.IsAuthenticated)
            {
                string companyEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userDetails = _databaseOperations.GetEmployees(companyEmail);
                return View(userDetails);
            }
           
            return View();
        }
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var user = _databaseOperations.GetUsersById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditEmployee(UserDetail user)
        {
            if (ModelState.IsValid)
            {
                _databaseOperations.UpdateUser(user);
                return RedirectToAction("ViewEmployees");
            }
            return View(user);
        }
        public IActionResult DeleteEmployee(int id)
        {
            var user = _databaseOperations.GetUsersById(id);
            if (user == null)
            {
                return NotFound();
            }
            bool deleteSuccess = _databaseOperations.deleteEmployee(user);
            if (deleteSuccess)
            {
                return RedirectToAction("ViewEmployees");

            }
            return View(user);
        }
    }
}
