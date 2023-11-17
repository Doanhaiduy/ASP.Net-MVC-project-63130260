using DATA_63130260;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Areas.Admin.Controllers
{
    public class DeletedItems_63130260Controller : Controller
    {
		// GET: Admin/DeletedItems_63130260

		[RoleAdmin]
		public ActionResult DeletedItems()
        {
			mapDeletedItems map = new mapDeletedItems();
			var list = map.getListDeletedProduct();
			return View(list);
        }

		[RoleAdmin]
		public ActionResult RestoreItemsProducts(int id)
		{
			mapDeletedItems map = new mapDeletedItems();
			if (map.RestoreItemsProducts(id))
			{
				return RedirectToAction("DeletedItems");
			}
			return View();
		}

		[RoleAdmin]
		public ActionResult RestoreItemsOrders(int id)
		{
			mapDeletedItems map = new mapDeletedItems();
			if (map.RestoreItemsOrder(id))
			{
				return RedirectToAction("DeletedItems");
			}
			return View();
		}
	}
}