using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DATA_63130260;
using DATA_63130260.Entity;
using Project_63130260.App_Start;

namespace Project_63130260.Controllers
{
    public class Product_63130260Controller : Controller
    {
        private Project_63130260Entities db = new Project_63130260Entities();
		site_user user = SessionConfig.GetUser(); // Lấy ra user của session hiện tại
		// GET: Product_63130260

		public ActionResult Index(string search, string sortBy, string priceFrom, string priceTo)
		{
            ViewBag.search = search;
            ViewBag.sortBy = sortBy;
            ViewBag.priceFrom = priceFrom;
            ViewBag.priceTo = priceTo;
			mapProduct map = new mapProduct();
			var listProdcut = map.getListProduct();
			if (!string.IsNullOrEmpty(search))
			{
				listProdcut = listProdcut.Where(m => m.product.name.ToLower().Contains(search.ToLower())).ToList();
			}
			if (!string.IsNullOrEmpty(sortBy))
            {
                listProdcut = map.SortProduct(sortBy, listProdcut);       
             }
            if (!string.IsNullOrEmpty(priceFrom))
            {
				listProdcut = listProdcut.Where(m => m.price >= Convert.ToInt32(priceFrom)).ToList();
			}
			if (!string.IsNullOrEmpty(priceTo))
			{
				listProdcut = listProdcut.Where(m => m.price <= Convert.ToInt32(priceTo)).ToList();
			}

			ViewBag.Count = listProdcut.Count;
				return View(listProdcut);
		}

		// GET: Product_63130260/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            mapProduct map = new mapProduct();
            product_item product_item = map.getDetailProduct((int)id);
            if (product_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.listProductRelated = map.getListProductOfCategory((int)product_item.product.category_id).ToList();
			return View(product_item);
        }

        public ActionResult Review(int id, int ordered_product_id)
        {
			ViewBag.ordered_product_id = ordered_product_id;
			mapProduct map = new mapProduct();
			product_item product_item = map.getDetailProduct((int)id);
			return View(product_item);
        }


		[HttpPost]		
		public ActionResult Review(user_review userReview)
		{
			mapReview map = new mapReview();
			userReview.user_id = user.id;
			map.AddNewReviewProduct(userReview);
			return RedirectToAction("ReviewSuccess");
		}

		public ActionResult ReviewSuccess() {
			return View();
		}
	}
}
