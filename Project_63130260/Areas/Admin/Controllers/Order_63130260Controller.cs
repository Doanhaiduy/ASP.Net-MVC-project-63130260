using DATA_63130260;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Areas.Admin.Controllers
{
    public class Order_63130260Controller : Controller
    {
		// GET: Admin/Order_63130260

		[RoleAdmin]
		public ActionResult Index()
        {
			if (TempData["error"] != null)
			{
				ViewBag.error = TempData["error"];
			}
			var map = new mapOrder();
			var listOrder = map.GetListOrder();
            
			return View(listOrder);
        }

		[RoleAdmin]
		public ActionResult Details(int id)
        {
			var map = new mapOrder();
            var order = map.GetDetailsOrder(id);
            return View(order);
		}

		[RoleAdmin]
		public ActionResult ConfirmSuccessDelivery(int id)
		{
			var map = new mapOrder();
			map.confirmOrder(id);
			return RedirectToAction("Index");
		}

		[RoleAdmin]
		public ActionResult DeleteOrder()
		{

			return View();
		}

		[RoleAdmin]
		[HttpPost]
		public ActionResult DeleteOrder(int id)
		{
			mapOrder map = new mapOrder();
			if (map.DeleteOrderById(id))
			{
				return RedirectToAction("index");
			}
			else
			{
				TempData["error"] = "Orders in transit cannot be deleted !";
				return RedirectToAction("Index");
			}
		}
	}
}