using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Domain.ViewModels;
using Common.Encryption;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using DatabaseAccess;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authorization;
using Common.JwtToken;


namespace Client.Controllers
{
    public class LoginController : Controller
	{
		private readonly DatabaseOperations _databaseOperations;
		private readonly IOptions<EncryptionSettings> _encryptionsettings;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly ILogger<LoginController> _logger;
		public LoginController(DatabaseOperations databaseOperations, IOptions<EncryptionSettings> encryptionsettings, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<LoginController> logger)
		{
			_configuration = configuration;
			_databaseOperations = databaseOperations;
            _encryptionsettings = encryptionsettings;
			_httpcontextAccessor = httpContextAccessor;
            _logger = logger;
		}


		[HttpGet]
		public IActionResult Login()
		{
          //  if (User.Identity != null && User.Identity.IsAuthenticated == true) return RedirectToAction("UserPage", "UserDashboard");
            
            return View();
		}
        [Authorize(Roles = "Admin")]
        [HttpGet]
		public IActionResult RegisterEmployee()
		{
			return View();
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
				try
				{
                    bool success = await _databaseOperations.StoreUser(user, passwordhash, passwordsalt);
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
		public async Task<IActionResult> CompanyRegistration(CompanyRegistration company)
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
		public IActionResult Login(LoginView user)
		{ 			
			if (ModelState.IsValid)
			{
				var (loginSuccess, isfirstTimeLogin) = _databaseOperations.CheckUser(user);//tuple return type multiple return values


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
		public IActionResult CompanyLogin(LoginView companyLogin)
		{
			if (ModelState.IsValid)
			{
				string errorMessage = _databaseOperations.CheckCompany(companyLogin);
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
                    return RedirectToAction("AdminPage", "CompanyAdmin");
                }
			}
			return View(companyLogin);
		}
		
		public IActionResult Logout()
		{
			Response.Cookies.Delete("jwtToken");
            _logger.LogInformation("User has Logged out");
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

