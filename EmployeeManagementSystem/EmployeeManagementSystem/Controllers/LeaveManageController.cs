using DatabaseAccess;
using Domain.Models;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmployeeManagementSystem.Controllers
{
    public class LeaveManageController : Controller
    {

        private readonly DatabaseOperations _databaseOperations;
        private readonly IDistributedCache _distributedCache;

        public LeaveManageController(DatabaseOperations databaseOperations, IDistributedCache distributedCache)
        {
            _databaseOperations = databaseOperations;
            _distributedCache = distributedCache;
        }

        // GET: LeaveManageController
        [HttpGet]
        [Authorize (Roles = "Admin")]
        public IActionResult CreateLeaves()
        {
            if (_distributedCache.GetString("UserID") != null)
            {
                var companyIDstring = _distributedCache.GetString("UserID");
                int companyID = JsonConvert.DeserializeObject<int>(companyIDstring);
                var leaves = _databaseOperations.ViewLeaves(companyID).Result;
                ViewBag.LeavesView = leaves;

            }
            else
            {
                TempData["Error"] = "UserID not found in cache. Login and Try again.";
            }
            return View();
            
        }
        [HttpGet]
        public IActionResult LeaveTable()
        {
            if (_distributedCache.GetString("UserID") != null)
            {
                var companyIDstring = _distributedCache.GetString("UserID");
                int companyID = JsonConvert.DeserializeObject<int>(companyIDstring);
                var leaves = _databaseOperations.ViewLeaves(companyID).Result;
                return PartialView("_LeaveTable", leaves);
            }
            return PartialView("_LeaveTable", new List<TypesOfLeave>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLeaves( AddLeaveView leave)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_distributedCache.GetString("UserID")!= null)
                    {
                        var companyIDstring = _distributedCache.GetString("UserID");
                        int companyID = JsonConvert.DeserializeObject<int>(companyIDstring);
                        bool success = _databaseOperations.AddLeaves(leave, companyID);
                        
                        
                        if (success)
                        {
                            var leaves = _databaseOperations.ViewLeaves(companyID).Result;
                            TempData["Leaves Added"] = "Leave was Successfully added. Check the table to see the Leaves.";
                            return PartialView("_LeaveTable", leaves);

                        }

                        else
                        {
                            throw new Exception("Not able to add Leave.");
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    TempData["Exception Error"] = ex.Message;
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Model state is invalid." });
        }

        [HttpPost]
        public IActionResult UpdateLeaveDays(int id, int days)
        {
            try
            {
                bool updatedSuccessfully = _databaseOperations.UpdateLeaves(id, days);
                if (!updatedSuccessfully)
                {
                    return Json(new { success = false, message = "Leave type not found." });
                }
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
