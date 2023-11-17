using DATA_63130260;
using DATA_63130260.Entity;
using Project_63130260.App_Start;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Controllers
{
    public class Account_63130260Controller : Controller
    {
		site_user user = SessionConfig.GetUser(); // Lấy ra user của session hiện tại
		// GET: Account_63130260
		[RoleUser]
		public ActionResult Index()
        {
			string error = TempData["error"] as string;
			ViewBag.Error = error;
			mapUser map = new mapUser();
			var currUser = map.GetDetailUser(user.id);
			return View(currUser);
        }

		[RoleUser]
		[HttpPost]
		public ActionResult UpdateInfo(site_user dataUser,string current_password, string new_password, string confirm_password)
		{
			mapUser map = new mapUser();
			if(map.ChangePassword(user.id, current_password, new_password, confirm_password) == true)
			{
				map.UpdateInfoUser(user.id, dataUser);
			}
			else
			{
				TempData["error"] = "The current password is incorrect or the confirmation password does not match!";
			}
			return RedirectToAction("Index");	
		}

		[RoleUser]
		[HttpPost]
		public ActionResult UpdateAddress(address dataAddress)
		{
			mapUser map =	new mapUser();
			var currUser = map.GetDetailUser(user.id);
			map.UpdateUserAddress(currUser.id, currUser, dataAddress);
			return RedirectToAction("Index");
		}

		[RoleUser]
		public ActionResult SetDefaulAddress(int idAddress)
		{
			mapAddress map = new mapAddress();
			map.SetDefaultAddressUser(user.id, idAddress);
			return RedirectToAction("Index");
		}
	}
}