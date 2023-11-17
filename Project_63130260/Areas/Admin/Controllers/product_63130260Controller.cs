using DATA_63130260;
using DATA_63130260.Entity;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Areas.Admin.Controllers
{
    public class product_63130260Controller : Controller
    {
		// GET: Admin/product_63130260

		[RoleAdmin]
		public ActionResult Index()
        {
            mapProduct map = new mapProduct();
            var list = map.getListProduct();
            return View(list);
        }

		[RoleAdmin]
		public ActionResult AddNewProduct()
		{
			mapProduct map = new mapProduct();
			return View();
		}

		[RoleAdmin]
		[HttpPost]
        public async Task<ActionResult> AddNewProduct(product product,int price, int qty_in_stock, HttpPostedFileBase product_image, HttpPostedFileBase product_image2, product_attributes proAttr)
        {
            FileStream stream;
            FileStream stream2;
			mapFirebase mapFirebase = new mapFirebase();
            // Kiểm tra xem có tồn tại file ảnh 1 hay k, nếu có thì xử lý upload ảnh lên firebase Storage
            if(product_image.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Content/assets/images"), product_image.FileName);
                product_image.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
               product.product_image = await Task.Run(() => mapFirebase.Upload(stream, product_image.FileName));
            }
			// Kiểm tra xem có tồn tại file ảnh 2 hay k, nếu có thì xử lý upload ảnh lên firebase Storage
			string imageItem = "";
			if (product_image2.ContentLength > 0)
			{
				string path = Path.Combine(Server.MapPath("~/Content/assets/images"), product_image2.FileName);
				product_image2.SaveAs(path);
				stream2 = new FileStream(Path.Combine(path), FileMode.Open);
				imageItem = await Task.Run(() => mapFirebase.Upload(stream2, product_image2.FileName));
			}

			mapProduct map = new mapProduct();
            map.AddNewProduct(product,price, qty_in_stock, imageItem, proAttr);
            return RedirectToAction("index");
        }

		[RoleAdmin]
		public ActionResult UpdateProduct(int id)
        {
            mapProduct map = new mapProduct();
            var productItem = map.getDetailProduct(id);
            return View(productItem);
        }

		[RoleAdmin]
		[HttpPost]
		public async Task<ActionResult> UpdateProduct(int id,product product, int price, int qty_in_stock, HttpPostedFileBase product_image, HttpPostedFileBase product_image2, product_attributes proAttr)
		{

			FileStream stream;
			FileStream stream2;
			mapFirebase mapFirebase = new mapFirebase();
			// Kiểm tra xem có tồn tại file ảnh 1 hay k, nếu có thì xử lý upload ảnh lên firebase Storage
			if (product_image != null)
			{
				if(product_image.ContentLength > 0)
				{
					string path = Path.Combine(Server.MapPath("~/Content/assets/images"), product_image.FileName);
					product_image.SaveAs(path);
					stream = new FileStream(Path.Combine(path), FileMode.Open);
					product.product_image = await Task.Run(() => mapFirebase.Upload(stream, product_image.FileName));
				}
			}
			else
			{
				product.product_image = null;
			}
		
			// Kiểm tra xem có tồn tại file ảnh 2 hay k, nếu có thì xử lý upload ảnh lên firebase Storage
			string imageItem = "";
			if (product_image2 != null)
			{
				if(product_image2.ContentLength > 0)
				{
					string path = Path.Combine(Server.MapPath("~/Content/assets/images"), product_image2.FileName);
					product_image2.SaveAs(path);
					stream2 = new FileStream(Path.Combine(path), FileMode.Open);
					imageItem = await Task.Run(() => mapFirebase.Upload(stream2, product_image2.FileName));
				}
			}
			else
			{
				imageItem = null;
			}
			mapProduct map = new mapProduct();
            product_item productItem = new product_item();
            productItem.price = price;
            productItem.qty_in_stock = qty_in_stock;
			productItem.product_image = imageItem;
			if (map.UpdateProduct(id, productItem, product, proAttr))
            {
			    return RedirectToAction("index");
            }
            return View();

		}

		[RoleAdmin]
		public ActionResult DeleteProduct()
		{
			return View();	
		}

		[RoleAdmin]
		[HttpPost]
		public ActionResult DeleteProduct(int id)
		{
			mapProduct map = new mapProduct();
			map.DeleteProduct(id);
			return RedirectToAction("index");
		}

		[RoleAdmin]
		[HttpPost]
		public ActionResult ImportMoreProduct(int idProduct, int qty)
		{
			mapProduct map = new mapProduct();
			map.ImportMoreProduct(idProduct, qty);
			return RedirectToAction("index");
		}

		public ActionResult DetailsProduct(int id)
		{
			mapProduct map = new mapProduct();
			var product = map.getDetailProduct(id);
			return View(product);
		}

	}
}