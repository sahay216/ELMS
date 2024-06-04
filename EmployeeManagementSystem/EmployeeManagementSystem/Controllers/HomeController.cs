using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Domain.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
        [Authorize]
		public IActionResult Index()
		{
            var UserClaims = User.Identity as ClaimsIdentity;
            var roles = UserClaims?.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminPage", "CompanyAdmin");
            }
            else if(roles.Contains("Employee")|| roles.Contains("Manager"))
            {
                return RedirectToAction("UserPage", "UserDashboard");
            }
			return RedirectToAction("Login", "Login");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
