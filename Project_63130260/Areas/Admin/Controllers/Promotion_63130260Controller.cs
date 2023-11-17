using DATA_63130260;
using DATA_63130260.Entity;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Areas.Admin.Controllers
{
    public class Promotion_63130260Controller : Controller
    {
		// GET: Admin/Promotion_63130260

		[RoleAdmin]
		public ActionResult Index()
        {
			mapPromotion map = new mapPromotion();
			var listPromotion = map.GetListPormotion();
            return View(listPromotion);
		}

		[RoleAdmin]
		public ActionResult AddNewPromotion()
		{
			return View();
		}

		[RoleAdmin]
		[HttpPost]
		public ActionResult AddNewPromotion(promotion promotion)
		{
			mapPromotion mapPromotion = new mapPromotion();
			if (mapPromotion.AddNewPromotion(promotion))
			{
				ViewBag.promotion = "";
				ViewBag.error = "";
				return RedirectToAction("index") ;
			}
			ViewBag.error = "An error occurred, please check the fields are valid!";
			ViewBag.promotion = promotion;
			return View();
		}

		[RoleAdmin]
		public ActionResult UpdatePromotion(int id)
		{
			mapPromotion map = new mapPromotion();
			var promotion = map.GetPromotionDetailsById(id);
			return View(promotion);
		}

		[RoleAdmin]
		[HttpPost]
		public ActionResult UpdatePromotion(int id, promotion newpromotion)
		{
			mapPromotion map = new mapPromotion();
			var promotion = map.GetPromotionDetailsById(id);
			if (map.UpdatePromotion(id, newpromotion))
			{
				ViewBag.error = "";
				return RedirectToAction("index");
			}
			ViewBag.error = "An error occurred, please check the fields are valid!";
			return View(promotion);
			
		}

		[RoleAdmin]
		public ActionResult DeactivatePromotion(int id)
		{
			mapPromotion map = new mapPromotion();
			map.DeactivePromotion(id); 
			return RedirectToAction("index");

		}

		[RoleAdmin]
		public ActionResult DeletePromotion()
		{
			return View();
		}

		[RoleAdmin]
		[HttpPost]
		public ActionResult DeletePromotion(int id)
		{
			mapPromotion map = new mapPromotion();
			map.DeletePromotion(id);
			return RedirectToAction("index");
		}


	}
}