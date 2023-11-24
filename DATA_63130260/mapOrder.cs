using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapOrder
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra tất cả đơn hàng hiện có trong hệ thống
		public List<shop_order> GetListOrder()
		{
			var listOrder = db.shop_order.OrderByDescending(m => m.order_date).Where(m => m.deleted == false).ToList();
			return listOrder;
		}
		// Lấy ra danh sách đơn hàng của 1 user
		public List<shop_order> GetListOrderByUser(int idUser)
		{
			var listOrder = db.shop_order.Where(m => m.user_id == idUser).OrderByDescending(m => m.order_date).Where(m => m.deleted == false).ToList();
			return listOrder;
		}

		// Lấy ra chi tiết đơn hàng theo id
		public shop_order GetDetailsOrder(int id)
		{
			var order = db.shop_order.Find(id);
			return order;
		}

		// Lấy ra các sản phẩm trong đơn hàng theo id
		public List<order_line> GetOrderItems(int id)
		{
			var orderItems = db.order_line.Where(m => m.order_id == id).ToList();
			return orderItems;
		}

		// Thêm sản phẩm vào đơn hàng 
		public void AddItemToOder(int idCart, int idOrder)
		{
			shop_order shop_Order = new shop_order();
			mapCart mapCart = new mapCart();
			// Lấy ra tất cả sản phẩm trong giỏ hàng hiện tại
			var listCartItem = mapCart.GetCartItems(idCart);
			List<order_line> order_Lines = new List<order_line>();
			// Thêm lần lượt các sản phẩm trong giỏ hàng thành đơn hàng
			foreach (var item in listCartItem)
			{
				if (db.product_item.Find(item.product_item_id).qty_in_stock - item.qty < 0)
				{
					// Nếu sản phẩm trong giỏ hàng nhiều hơn sản phẩm trong kho thì bỏ qua không thêm vào đơn
					continue;
				}
				else
				{
					db.product_item.Find(item.product_item_id).qty_in_stock -= item.qty; // Cập nhật lại số lượng hàng trong kho
					db.product_item.Find(item.product_item_id).qty_sold += item.qty; // Cập nhật lại số lượng hàng đã bán
																					 // Tạo mới từng sản phẩm đơn hàng
					order_line newOrderLine = new order_line();
					newOrderLine.order_id = idOrder;
					newOrderLine.product_item_id = item.product_item_id;
					newOrderLine.qty = item.qty;
					newOrderLine.reviewed = false;
					// Giữ lại giá của sản phẩm để tránh trường hợp sản phẩm bị đổi giá khi dẫn tới đơn hàng bị đổi giá
					newOrderLine.price = mapPromotion.CheckAndCalculatePrice(item.product_item);
					var discountProduct = mapPromotion.GetPromotionDetails((int)item.product_item.product.category_id);
					// Kiểm tra xem sản phẩm đó có đang trong chương trình khuyến mãi hay không, nếu có thì cập nhật mục discount_rate, k thì bằng 0;
					if (discountProduct != null)
					{
						newOrderLine.discount_rate = discountProduct.discount_rate;
					}
					else
					{
						newOrderLine.discount_rate = 0;
					}
					order_Lines.Add(newOrderLine);
					// Xóa tất cả các sản phẩm trong giỏ hàng
					mapCart.DeleteCartItem((int)item.cart_id, item.product_item.id);
				}
				// Thêm danh sách sản phẩm đơn hàng vào db
			}
			db.order_line.AddRange(order_Lines);
			db.SaveChanges();
		}
		
		// Thêm đơn hàng mới
		public async Task<shop_order> AddNewOrder(int idCart, shop_order order, int idAddressOrder = 0, decimal tolal = 0)
		{
			shop_order newOrder = new shop_order();
			newOrder.user_id = order.user_id;
			newOrder.order_date = mapDateTime.GetVietnamDateTime();
			newOrder.order_first_name = order.order_first_name;
			newOrder.order_last_name = order.order_last_name;
			newOrder.order_phone = order.order_phone;
			newOrder.payment_method_id = order.payment_method_id;
			newOrder.shipping_method = order.shipping_method;
			if (idAddressOrder != 0)
			{
				newOrder.shipping_address = idAddressOrder;
			}
			else
			{
				newOrder.shipping_address = order.shipping_address;
			}
			newOrder.order_note = order.order_note;
			newOrder.deleted = false;
			newOrder.order_status = false;
			db.shop_order.Add(newOrder);
			db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
			AddItemToOder(idCart, newOrder.id);
			// Tính tổng tiền của đơn hàng

			//kiểm tra xem total có bằng 0 hay k (nếu bằng 0 thì không có thay đổi (giảm giá)
			if (tolal != 0)
			{
				newOrder.order_total = tolal + db.shipping_method.Find(order.shipping_method).price;
			}
			else
			{
				newOrder.order_total = db.order_line.Where(m => m.order_id == newOrder.id).Sum(m => m.price * m.qty) + db.shipping_method.Find(order.shipping_method).price;
			}
			await db.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
			return newOrder;
		}

		// Xác nhận đã giao thành công đơn hàng
		public void confirmOrder(int id)
		{
			var order = db.shop_order.Find(id);
			order.order_status = true;
			db.SaveChanges();
		}

		// Xóa đơn hàng 
		public bool DeleteOrderById(int id)
		{
			var order = db.shop_order.Find(id);
			if (order != null && order.order_status == true)
			{
				order.deleted = true;
				db.SaveChanges();
				return true;
			}
			return false;
		}

	}
}
