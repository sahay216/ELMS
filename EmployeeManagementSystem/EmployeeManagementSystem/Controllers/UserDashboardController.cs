using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Action_Atributes;
using Domain.Models;
using DatabaseAccess;
using Domain.ViewModels;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmployeeManagementSystem.Controllers
{
    
    public class UserDashboardController : Controller
    {
        private readonly DatabaseOperations _databaseOperations;
        private readonly IDistributedCache _distributedCache;
        public UserDashboardController(DatabaseOperations databaseOperations, IDistributedCache distributedCache)
        {
            _databaseOperations = databaseOperations;
            _distributedCache = distributedCache;
        }


        [Authorize(Roles ="Employee,Manager")]
        [HttpGet]
        [NoCache]
        public IActionResult UserPage()
        {
            
            if (string.IsNullOrEmpty(_distributedCache.GetString("BirthdayEmployee")))
            {
                 var UserBirthday = _databaseOperations.GetBirthdayEmployees();
                _distributedCache.SetString("BirthdayEmployee", JsonConvert.SerializeObject(UserBirthday), new DistributedCacheEntryOptions { 
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });

                ViewBag.UserBirthdayList = UserBirthday;
            }
            else
            {
                var BDusersFromString = _distributedCache.GetString("BirthdayEmployee");
                ViewBag.UserBirthdayList = JsonConvert.DeserializeObject<List<UserDetailDashboard>>(BDusersFromString);
            }

            if (string.IsNullOrEmpty(_distributedCache.GetString("NewHires")))
            {
                var newHires = _databaseOperations.GetNewHires();

                _distributedCache.SetString("NewHires", JsonConvert.SerializeObject(newHires), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                ViewBag.newHireUsers = newHires;
            }
            else
            {
                var NHusersFromString = _distributedCache.GetString("NewHires");
                ViewBag.newHireUsers = JsonConvert.DeserializeObject<List<UserDetailDashboard>>(NHusersFromString);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public IActionResult EmployeeSearch(string query)
        {
            try
            {
                var user = _databaseOperations.SearchEmployee(query);
                
                if (user == null || !user.Any())
                {
                    return PartialView("_NoResult");
                }
                return View(user);
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

            string? userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _databaseOperations.UserProfile(userEmail);
            return View(user);
        }
        public IActionResult ApplyLeaves()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn()
        {
            var useridstring = _distributedCache.GetString("UserID");
            int UserID = JsonConvert.DeserializeObject<int>(useridstring);
            var attendance =  _databaseOperations.StoreAttendanceCheckin(UserID);

            return Json(new { success = attendance != null });
        }
        public async Task<IActionResult> CheckOut()
        {
            var useridstring = _distributedCache.GetString("UserID");
            int UserID = JsonConvert.DeserializeObject<int>(useridstring);
            var attendance =  _databaseOperations.StoreAttendanceCheckout(UserID);

            return Json(new { success = attendance != null });
        }

        public IActionResult ApplyLeaves(LeaveApplication leave)
        {
            return View();
        }

        
    }
}
