using DATA_63130260;
using DATA_63130260.Entity;
using Project_63130260.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Controllers
{
    public class Auth_63130260Controller : Controller
    {
        // GET: Auth_63130260
        public ActionResult Login()
        {
			// Điều hướng về trang chủ khi truy cập trang login mà vẫn còn session người dùng
			string returnUrl = Session["ReturnUrlUser"] as string;
			var userSS = SessionConfig.GetUser();
			if (userSS != null)
			{
				return Redirect("/");
			}
			return View();
		}

		[HttpPost]
		public ActionResult Login(string username, string password)
		{
			

			// Kiểm tra thông tin đăng nhập
			mapUser map = new mapUser();
			var user = map.CheckUserLogin(username, password);
			if (user != null)
			{
				SessionConfig.SetUser(user);
				return Redirect("/"); // Nếu thông tin đăng nhập đúng thì set cho session là user đã đăng nhập và đưa về trang chủ 
			}
			ViewBag.username = username;
			ViewBag.error = "Username or password is incorrect";
			return View();
		}

		public ActionResult Register()
		{
			// Điều hướng về trang chủ khi truy cập trang register mà vẫn còn session người dùng
			var userSS = SessionConfig.GetUser();
			if (userSS != null)
			{
				return Redirect("/");
			}
			return View();
		}

        [HttpPost]
		public ActionResult Register(site_user user)
		{
			ViewBag.first_name = user.first_name;
			ViewBag.last_name = user.last_name;
			ViewBag.email_address = user.email_address;
			mapUser map = new mapUser();
			if(map.CreateUser(user) == true)
			{
				return RedirectToAction("login");
			}
			else
			{
				ViewBag.error = "Email already exists in the system!";
				return View();
			}
		}

		public ActionResult Logout()
		{
			SessionConfig.SetUser(null);
			return RedirectToAction("Login");
		}
	}
}