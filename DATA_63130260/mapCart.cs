using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapCart
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		public shopping_cart CreateCart(int idUser)
		{
			// Tạo giỏ hàng mới tương ứng với 1 khách hàng
			var newCart = new shopping_cart();
			newCart.user_id = idUser;
			db.shopping_cart.Add(newCart);
			db.SaveChanges();
			return newCart;
		}

		public shopping_cart GetCart(int idUser)
		{
			return db.shopping_cart.FirstOrDefault(m => m.user_id == idUser); // Tìm giỏ hàng trùng với id người dùng
		}

		
		public List<shopping_cart_item> GetCartItems(int idCart)
		{
			var list = db.shopping_cart_item.Where(m => m.cart_id == idCart && m.product_item.product.deleted == false).ToList(); // Lấy ra tất cả các sản phẩm trong giỏ hàng có id = idCart
			return list;
		}

		public double GetToTalCart(int idCart)
		{
			var total = db.shopping_cart_item.Where(m => m.cart_id == idCart && m.product_item.product.deleted == false).Sum(m => m.product_item.price * m.qty);
			return (double)total;
		}

		public void AddNewCartItem(int idUser, int idProduct, int quantity) //Thêm sản phẩm vào giỏ hàng
		{
			var newItem = new shopping_cart_item();
			var cart = db.shopping_cart.FirstOrDefault(m => m.user_id == idUser);
			// Kiểm tra xem sản phẩm này có tồn tại trong giỏ hàng hay chưa 
			var item = db.shopping_cart_item.FirstOrDefault(m => m.product_item_id == idProduct & m.cart_id == cart.id);
			if(item == null) // Nếu chưa tồn tại thì thêm vào bảng shopping_cart_item
			{
				newItem.cart_id = cart.id;
				newItem.product_item_id = idProduct;
				newItem.qty = quantity;
				db.shopping_cart_item.Add(newItem);
				db.SaveChanges();
			}
			else
			{
				// Nếu tồn tại rồi thì chỉ cần tăng số lượng sản phẩm trong giỏ hàng lên
				item.qty = item.qty + quantity;
				db.SaveChanges();
			}
			
		}

		public bool DeleteCartItem(int idCart, int idProduct)
		{
			// tìm sản phẩm cần xóa khỏi bảng shopping_cart_item trong db
			var item = db.shopping_cart_item.FirstOrDefault(m => m.cart_id == idCart & m.product_item_id == idProduct);
			if (item != null)
			{
				// nếu tìm thấy thì thực hiện xóa sản phẩm đó khỏi db
				db.shopping_cart_item.Remove(item);
				db.SaveChanges();
				return true;
			}
			else
			{
				// nếu tìm không thấy thì trả về false và kết thúc
				return false;
			}
		}

		public void DeleteAfterCheckout(int idCart)
		{
			// Xóa tất cả các sản phẩm trong giỏ hàng sau khi thanh toán
			var ListSanPhamXoa = db.shopping_cart_item.Where(m => m.cart_id == idCart).ToList();
			db.shopping_cart_item.RemoveRange(ListSanPhamXoa);
			db.SaveChanges();
		}

		// Lấy ra tổng tiền trong giỏ hàng
		public double GetTotalCart(int idCart)
		{
			var total = GetCartItems(idCart).Where(m => m.cart_id == idCart).Sum(m => m.qty * m.product_item.price);
			return (double)total;
		}


	}
}
