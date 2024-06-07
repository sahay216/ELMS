using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Action_Atributes;
using Domain.Models;
using DatabaseAccess;
using Domain.ViewModels;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Common.RedisImplementation;

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
        public async Task<IActionResult> UserPage()
        {

            var redisService = new RedisService(_distributedCache);
            var UserBirthday = redisService.GetValue<List<UserDetailDashboardView>>(RedisKey.BirthdayEmployee);
            if (UserBirthday == null)
            {

                UserBirthday = await _databaseOperations.GetBirthdayEmployees();
                redisService.SetValue(RedisKey.BirthdayEmployee, UserBirthday, TimeSpan.FromMinutes(10));
            }
            ViewBag.UserBirthdayList = UserBirthday;

            var newHires = redisService.GetValue<List<UserDetailDashboardView>>(RedisKey.NewHires);
            if (newHires == null)
            {
                 newHires = await _databaseOperations.GetNewHires();
                redisService.SetValue(RedisKey.NewHires, newHires, TimeSpan.FromMinutes(10));
            }
            ViewBag.newHireUsers = newHires;

            var publicHolidays = redisService.GetValue<List<HolidayView>>(RedisKey.PublicHolidays);
            if (publicHolidays == null)
            {
                publicHolidays = await _databaseOperations.GetPublicHolidays();
                redisService.SetValue(RedisKey.PublicHolidays, publicHolidays, TimeSpan.FromMinutes(10));
            }
            ViewBag.publicHolidays = publicHolidays;
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
        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userDetails = _databaseOperations.GetUsersById(id);
                return RedirectToAction("EditEmployee","CompanyAdmin",userDetails);
            }

            return View();
        }
        [HttpGet]
        public IActionResult ApplyLeaves()
        {
            var redisService = new RedisService(_distributedCache);
            var userID = redisService.GetValue<int>(RedisKey.UserID);

            var userDetail = _databaseOperations.GetUsersById(userID);
            if (userDetail == null)
            {
                return View();
            }
            var leaveBalance = _databaseOperations.GetLeavesByUserID(userID);

            var viewModel = new ApplyLeaveView
            {
                UserDetail = userDetail,
                UserLeaveBalances = leaveBalance
            };
            return View(viewModel);
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
        [HttpPost]
        public IActionResult ApplyLeaves(ApplyLeaveView leave)
        {
                
            return View();
        }

        
    }
}
