using DATA_63130260;
using Project_63130260.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Areas.Admin.Controllers
{
    public class Auth_63130260Controller : Controller
    {
		// GET: Admin/Auth_63130260
		public ActionResult Login()
		{
			// Điều hướng về trang chủ khi truy cập trang login mà vẫn còn session người dùng
			string returnUrl = Session["ReturnUrlAdmin"] as string;
			var adminSS = SessionConfig.GetAdmin();
			if (adminSS != null)
			{
				return Redirect("/admin");
			}
			return View();
		}
		[HttpPost]
		public ActionResult Login(string username, string password)
		{


			// Kiểm tra thông tin đăng nhập
			mapAdmin map = new mapAdmin();
			var admin = map.CheckAdminLogin(username, password);
			if (admin != null)
			{
				SessionConfig.SetAdmin(admin);
				return Redirect("/admin"); // Nếu thông tin đăng nhập đúng thì set cho session là admin đã đăng nhập và đưa về trang chủ  admin
			}
			ViewBag.username = username;
			ViewBag.error = "Username or password is incorrect";
			return View();
		}

		public ActionResult Logout()
		{
			SessionConfig.SetAdmin(null);
			return RedirectToAction("Login");
		}

	}
}