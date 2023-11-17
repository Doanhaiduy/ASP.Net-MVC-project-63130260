using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapDeletedItems
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Hiển thị danh sách sản phẩm đã bị xóa
		public List<product_item> getListDeletedProduct()
		{
			var list = db.product_item.OrderByDescending(m => m.id).Where(m => m.product.deleted == true).ToList();
			return list;
		}

		// Hiển thị danh sách đơn hàng đã bị xóa
		public List<shop_order> getListDeletedOrder()
		{
			var list = db.shop_order.OrderByDescending(m => m.id).Where(m => m.deleted == true).ToList();
			return list;
		}


		// Khôi phục sản phẩm đã xóa
		public bool RestoreItemsProducts(int id)
		{
			var product = db.products.Find(id);
			if (product == null) { return false; }
			product.deleted = false;
			db.SaveChanges();
			return true;
		}

		// Khôi phục đơn hàng đã xóa
		public bool RestoreItemsOrder(int id)
		{
			var order = db.shop_order.Find(id);
			if(order == null) { return false;  }
			order.deleted = false;
			db.SaveChanges();
			return true;
		}
	}
}
