using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_63130260.App_Start
{
	public class SessionConfig
	{
		//User
		// Lưu session cho user
		public static void SetUser(site_user user)
		{
			// lưu vào session
			HttpContext.Current.Session["user"] = user;
		}

		// Lấy session
		public static site_user GetUser()
		{
			// lưu vào session
			return (site_user)HttpContext.Current.Session["user"];
		}

		// Amin
		public static void SetAdmin(site_admin admin)
		{
			// lưu vào session
			HttpContext.Current.Session["admin"] = admin;
		}

		// Lấy session
		public static site_admin GetAdmin()
		{
			// lưu vào session
			return (site_admin)HttpContext.Current.Session["admin"];
		}
	}
}