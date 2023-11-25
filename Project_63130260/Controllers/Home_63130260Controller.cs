using DATA_63130260;
using DATA_63130260.Entity;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1.X509;
using PayPal.Api;
using Project_63130260.App_Start;
using Project_63130260.Models;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Controllers
{
	public class Home_63130260Controller : Controller
	{

		site_user user = SessionConfig.GetUser(); // Lấy ra user của session hiện tại
		public ActionResult Index()
		{
			mapUser mapUser = new mapUser();
			//mapUser.dongbo();
			// Tạo session để lưu lại sản phẩm trong mục so sánh
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item>;

			if (comparedProducts == null)
			{
				// Nếu session chưa tồn tại, tạo một danh sách trống.
				comparedProducts = new List<product_item>();
				Session["ComparedProducts"] = comparedProducts;
			}
			mapProduct map = new mapProduct();
			var list = map.getListProduct();
			return View(list);
		}

		[RoleUser]
		public ActionResult Cart()
		{
			mapCart map = new mapCart();
			var cart = map.GetCart(user.id);
			var listCartItems = map.GetCartItems(cart.id);
			return View(listCartItems);
		}

		[RoleUser]
		[HttpPost]
		public ActionResult AddProductToCart(int idProduct, int qty = 1)
		{
			mapCart map = new mapCart();
			map.AddNewCartItem(user.id, idProduct, qty);
			return RedirectToAction("Cart");
		}

		[RoleUser]
		[HttpPost]
		public ActionResult DeleteProductCart(int idProduct)
		{
			mapCart map = new mapCart();
			map.DeleteCartItem(map.GetCart(user.id).id, idProduct);
			return RedirectToAction("Cart");
		}

		[RoleUser]
		public ActionResult DetailsOrder(int id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			mapOrder map = new mapOrder();
			var order = map.GetDetailsOrder(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}


		public ActionResult Whish()
		{

			return View();
		}

		public ActionResult Compare()
		{
			// Lấy danh sách sản phẩm so sánh từ Session (nếu đã tồn tại).
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item> ?? new List<product_item>();
			return View(comparedProducts);
		}

		public ActionResult AddToCompare(int id)
		{
			// Lấy danh sách sản phẩm so sánh từ Session (nếu đã tồn tại).
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item> ?? new List<product_item>();

			// Tìm sản phẩm theo ID trong cơ sở dữ liệu và thêm vào danh sách so sánh.
			mapProduct map = new mapProduct();
			var productCompare = map.getDetailProduct(id);
			if (productCompare != null)
			{
				// Chỉ giới hạn so sánh 2 sản phẩm, nếu đã có 2 sản phẩm trong mục so sánh thì sẽ thay thế sản phẩm này bằng sản phẩm đầu
				if (comparedProducts.Count == 2)
				{
					comparedProducts.Remove(comparedProducts[0]);
				}
				comparedProducts.Add(productCompare);
				// Lưu danh sách sản phẩm so sánh vào Session.
				Session["ComparedProducts"] = comparedProducts;
			}
			return RedirectToAction("Compare");
		}

		// Xóa sản phẩm so sánh
		public ActionResult RemoveFromCompare(int id)
		{
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item>;

			if (comparedProducts != null)
			{
				product_item product = comparedProducts.FirstOrDefault(p => p.id == id);

				if (product != null)
				{
					comparedProducts.Remove(product);
					Session["ComparedProducts"] = comparedProducts;
				}
			}

			return RedirectToAction("Compare");
		}
	}
}