using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Action_Atributes;
using Domain.Models;
using DatabaseAccess;
using Domain.ViewModels;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    
    public class UserDashboardController : Controller
    {
        private readonly DatabaseOperations _databaseOperations;
        public UserDashboardController(DatabaseOperations databaseOperations)
        {
            _databaseOperations = databaseOperations;
        }


        [Authorize(Roles ="Employee,Manager")]
        [HttpGet]
        [NoCache]
        public IActionResult UserPage()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public IActionResult Search(string query)
        {
            try
            {
                var user = _databaseOperations.SearchEmployee(query);
                if (user == null)
                {
                    throw new InvalidOperationException("No users found");
                }
                return RedirectToAction();
            }
            catch(InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("SearchResults", new List<ViewEmployee>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An unexpected error occurred.";
                return View("Error");
            }
        }

        public IActionResult UserProfile()
        {

            string userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _databaseOperations.UserProfile(userEmail);
            return View(user);
        }
    }
}
