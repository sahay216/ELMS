using EmployeeManagementSystem.Action_Atributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatabaseAccess;
using Domain.Models;
using System.Security.Claims;
using Domain.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace EmployeeManagementSystem.Controllers
{
    public class CompanyAdminController : Controller
    {
        private readonly DatabaseOperations _databaseOperations;
        private readonly IDistributedCache _distributedCache;

        public CompanyAdminController(DatabaseOperations databaseOperations, IDistributedCache distributedCache)
        {
            _databaseOperations = databaseOperations;
            _distributedCache = distributedCache;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        [NoCache]
        public async Task<IActionResult> AdminPage()
        {
            if (string.IsNullOrEmpty(_distributedCache.GetString("NewHires")))
            {
                var newHires = await _databaseOperations.GetNewHires();

                _distributedCache.SetString("NewHires", JsonConvert.SerializeObject(newHires), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                ViewBag.newHireUsers = newHires;
            }
            else
            {
                var NHusersFromString = _distributedCache.GetString("NewHires");
                ViewBag.newHireUsers = JsonConvert.DeserializeObject<List<UserDetailDashboardView>>(NHusersFromString);
            }


            if (string.IsNullOrEmpty(_distributedCache.GetString("LeaveReport")))
            {
                if (_distributedCache.GetString("UserID") != null)
                {
                    var companyIDstring = _distributedCache.GetString("UserID");
                    int companyID = JsonConvert.DeserializeObject<int>(companyIDstring);
                    var allLeaves = await _databaseOperations.GetLeaveReport(companyID);

                    _distributedCache.SetString("LeaveReport", JsonConvert.SerializeObject(allLeaves), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                    ViewBag.leaveReport = allLeaves;
                }
            }
            else
            {
                ViewBag.leaveReport = JsonConvert.DeserializeObject<List<LeaveReportView>>(value: _distributedCache.GetString("LeaveReport"));
            }

            return View();
        }
        
        [HttpGet]
        public IActionResult EmployeeRegistration()
        {
            return RedirectToAction("RegisterEmployee", "Login");
        }
        [HttpGet]
        public IActionResult ViewProfile()
        {
            if (_distributedCache.GetString("UserID") != null)
            {
                var companyIDstring = _distributedCache.GetString("UserID");
                int companyID = JsonConvert.DeserializeObject<int>(companyIDstring);
                var company = _databaseOperations.GetCompanyDetails(companyID);
                if(company!= null)
                {
                    return View(company);
                }
            }
            return View(new CompanyProfileView());
               
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
        public IActionResult ManageLeaves()
        {
            return View();
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
            bool deleteSuccess = _databaseOperations.DeleteEmployee(user);
            if (deleteSuccess)
            {
                return RedirectToAction("ViewEmployees");

            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> ViewLeaves()
        {
            if (_distributedCache.GetString("UserID")!= null)
            {
                var companyIDstring = _distributedCache.GetString("UserID");
                int companyID = JsonConvert.DeserializeObject<int>(companyIDstring);
                return PartialView("_LeaveReport", await _databaseOperations.GetLeaveReport(companyID));
            }
            return PartialView("_LeaveReport", new List<LeaveReportView>());
        }
        
    }
}
