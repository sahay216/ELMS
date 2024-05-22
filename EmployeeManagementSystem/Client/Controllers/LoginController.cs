using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Domain.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Client.Controllers
{
	public class LoginController : Controller
	{
		SqlConnection conn = new SqlConnection();
		SqlCommand cmd = new SqlCommand();
		SqlDataReader dr;
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}
		void connectionString()
		{
			conn.ConnectionString = "data source= ELTP-LAP-0185\\SQLEXPRESS; database=EmployeeTracker; integrated security = SSPI; ";
		}
		public ActionResult Signup()
		{
			return View();
		}
		public ActionResult Verify(User user) 
		{
			connectionString();
			conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = "select * from Users where email='"+user.Email+"'and password ='"+user.;
			dr= cmd.ExecuteReader();
			if(dr.Read())
			{
				return View(user);
			}
			else
			{
				return View();
			}
			conn.Close();
			return View();
		}
	}
}
