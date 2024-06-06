using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.ViewModels;
using Common.Encryption;
using Common.RedisImplementation;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using DatabaseAccess;
using Microsoft.AspNetCore.Authorization;
using Common.JwtToken;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;


namespace Client.Controllers
{
    public class LoginController : Controller
	{
		private readonly DatabaseOperations _databaseOperations;
		private readonly IOptions<EncryptionSettings> _encryptionsettings;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly ILogger<LoginController> _logger;
        private readonly IDistributedCache _distributedCache;

        //private readonly RegistrationService _registrationService;
        public LoginController(DatabaseOperations databaseOperations, IOptions<EncryptionSettings> encryptionsettings, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<LoginController> logger, IDistributedCache distributedCache)
        {
            _configuration = configuration;
            _databaseOperations = databaseOperations;
            _encryptionsettings = encryptionsettings;
            _httpcontextAccessor = httpContextAccessor;
            _logger = logger;
            _distributedCache = distributedCache;
        }


        [HttpGet]
		public IActionResult Login()
		{
          //  if (User.Identity != null && User.Identity.IsAuthenticated == true) return RedirectToAction("UserPage", "UserDashboard");
            
            return View();
		}
        [Authorize(Roles = "Admin")]
        [HttpGet]
		public async Task<IActionResult> RegisterEmployee()
		{
            var companyidstring = _distributedCache.GetString("UserID");
            int CompanyID = JsonConvert.DeserializeObject<int>(companyidstring);
            var leaveTypes = await _databaseOperations.GetLeaveReport(CompanyID);
            var leaveBalance = new List<LeaveBalanceView>();
            foreach (var leaveType in leaveTypes)
            {
                leaveBalance.Add(new LeaveBalanceView
                {
                    LeaveTypeID = leaveType.LeaveID,
                    LeaveName = leaveType.LeaveName,
                    AllotedDays = leaveType.AllotedDays
                });     
            }
            var registrationModel = new RegistrationView { AddLeaveViews = leaveBalance };
			return View(registrationModel);
		}
        public IActionResult CompanyRegistration()
		{
            if (User.Identity != null && User.Identity.IsAuthenticated == true) return RedirectToAction("UserPage", "UserDashboard");
            return View();
		}
		public IActionResult CompanyLogin()
		{
            if (User.Identity != null && User.Identity.IsAuthenticated == true) return RedirectToAction("UserPage", "UserDashboard");
            return View();
		}
        public IActionResult ForgotPassword()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> RegisterEmployee(RegistrationView user)
		{
            ModelState.Clear();

			var validationResults = user.Validate(new ValidationContext(user));
			foreach (var validationResult in validationResults)
			{
				ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
			}

            if (ModelState.IsValid)
			{
				string encryptionkey = _encryptionsettings.Value.EncryptionKey;
				var AesEncryptor = new AesEncryptor(Options.Create(_encryptionsettings.Value));
				var passwordhash =  AesEncryptor.Encrypt(user.Password, out string passwordsalt);
                var companyidstring = _distributedCache.GetString("UserID");
                int CompanyID = JsonConvert.DeserializeObject<int>(companyidstring);
				try
				{
                    bool success = await _databaseOperations.StoreUser(user, passwordhash, passwordsalt, CompanyID);
                    if (success)
                    {
                        TempData["Registered"] = "Succes";
                        return RedirectToAction("EmployeeRegistration", "CompanyAdmin");
                    }

                }
				catch (InvalidOperationException ex)
				{
					TempData["ErrorMessage"] = ex.Message;
					return RedirectToAction("RegisterEmployee");
				}
            }
			return View(user);
		}
		[HttpPost]
		public async Task<IActionResult> CompanyRegistration(CompanyRegistrationView company)
		{
			ModelState.Clear();
            var validationResults = company.Validate(new ValidationContext(company));
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
			
            if (ModelState.IsValid)
			{
				string encryptionkey = _encryptionsettings.Value.EncryptionKey;
				var AesEncryptor = new AesEncryptor(Options.Create(_encryptionsettings.Value));
				var passwordhash = AesEncryptor.Encrypt(company.Password, out string passwordsalt);
				try
				{
					bool success = await _databaseOperations.StoreCompany(company, passwordhash, passwordsalt);
					if (success)
					{
						return RedirectToAction("CompanyLogin");
					}
				}
				catch(InvalidOperationException ex)
				{
					TempData["ErrorMessage"] = ex.Message;
					return RedirectToAction("CompanyRegistration");
				}
			}
			return View(company);
		}
		  
		[HttpPost]
		public async Task<IActionResult> Login(LoginView user)
		{ 			
			if (ModelState.IsValid)
			{
				var (loginSuccess, isfirstTimeLogin) =  await _databaseOperations.CheckUser(user);//tuple return type multiple return values


                if (loginSuccess)
                {
                    if (isfirstTimeLogin)
                    {
                        _logger.LogInformation($"First Time Logging for {user.Email}");
                        TempData["First Time login"] = "FirstTimeLogin";
                        return RedirectToAction("ForgotPassword");
                    }


					var Role = _databaseOperations.GetRoleName(user.Email);
					var tokenString = JwtGenerate.GenerateJWT(user, _configuration,Role);
					HttpContext.Response.Cookies.Append("jwtToken", tokenString, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict,
						Expires = DateTime.Now.AddMinutes(5)
					}) ;
                    _logger.LogInformation($"Login was Successful for user {user.Email}");


                    var radisService = new RedisService(_distributedCache);
                    var UserID = await _databaseOperations.GetUserID(user.Email);
                    radisService.SetValue(RedisKey.UserID, UserID, TimeSpan.FromDays(10));


                    TempData["LoginSuccess"] = "Login Successful";
					return RedirectToAction("UserPage","UserDashboard");
                }	
				else
				{
                    TempData["LoginFail"] = "Invalid Email/Password entered, Kindly try again!";
                    _logger.LogError($"Login failed for user {user.Email}");
                    ModelState.AddModelError("", "User Not Found");
				}
			}
			
			return View(user);
		}
		[HttpPost]
		public async Task<IActionResult> CompanyLogin(LoginView companyLogin)
		{
			if (ModelState.IsValid)
			{
				string errorMessage = await _databaseOperations.CheckCompany(companyLogin);
				if (errorMessage!= null)
				{
					TempData["LoginError"] = errorMessage;
					return RedirectToAction("CompanyLogin");
					
				}
				else
				{

                    string Role = "Admin";
                    var tokenString = JwtGenerate.GenerateJWT(companyLogin, _configuration, Role);
                    HttpContext.Response.Cookies.Append("jwtToken", tokenString, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,  
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddMinutes(5)
                    });
                    var radisService = new RedisService(_distributedCache);
                    var UserID = await _databaseOperations.GetAdminID(companyLogin.Email);
                    radisService.SetValue(RedisKey.UserID, UserID);
                    return RedirectToAction("AdminPage", "CompanyAdmin");
                }
			}
			return View(companyLogin);
		}
		
		public IActionResult Logout()
		{
			Response.Cookies.Delete("jwtToken");
            _logger.LogInformation("User has Logged out");

            var radisService = new RedisService(_distributedCache);
            radisService.DeleteString(RedisKey.UserID);
			return RedirectToAction("Login");
		}
        [HttpPost]
		public IActionResult ForgotPassword(ForgotPasswordView forgotPassword)
		{
            if (ModelState.IsValid)
            {
                var AesEncryptor = new AesEncryptor(Options.Create(_encryptionsettings.Value));
                var passwordhash = AesEncryptor.Encrypt(forgotPassword.NewPassword, out string passwordsalt);
                try
                {

                    bool success = _databaseOperations.UpdatePassword(forgotPassword, passwordhash, passwordsalt);
                    if (success)
                    {
                        TempData["Password Updated"] = "Successfully Updated Passoword";
                        return RedirectToAction("Login");
                    }
                }
                catch(InvalidDataException ex) 
                {
                    TempData["Error Message"] = ex.Message;
                    return RedirectToAction("ForgotPassword");
                }
            }
            return View();
		}

    }
}

