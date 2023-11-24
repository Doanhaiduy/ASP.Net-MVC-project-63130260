using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapPromotion
	{
		public Project_63130260Entities db = new Project_63130260Entities();
		// Lấy ra danh sách khuyến mãi

		public List<promotion> GetListPormotion()
		{
			var list = db.promotions.OrderByDescending(m => m.start_date).ToList();
			return list;
		}



		// Thêm khuyến mãi cho danh mục
		public string AddNewPromotion(promotion promotion)
		{
			var checkPromotion = db.promotions.FirstOrDefault(m => m.category_id == promotion.category_id);
			// Kiểm tra xem danh mục đó đã có trong chương trình khuyến mãi chưa, nếu có thì không làm gì cả
			if (checkPromotion != null)
			{
				return "This category is part of another promotion.";
			}
			// Nếu ngày bắt đầu khuyến mãi diễn ra sau ngày kết thúc khuyến mãi thì trả về false (vô lý)
			if(promotion.start_date > promotion.end_date)
			{
				return "The end date must be greater than the start date.";
			}
			promotion.active = true;
			db.promotions.Add(promotion);
			db.SaveChanges();
			return null;
		}

		// kiểm tra và tính giá sản phẩm sau khi giảm giá 
		public static decimal CheckAndCalculatePrice(product_item item)
		{
			DateTime vietnamDateTime = mapDateTime.GetVietnamDateTime();
			Project_63130260Entities db = new Project_63130260Entities();
			// lọc qua bảng promotions xem có chuognw trình khuyến mãi nào có id danh mục bằng với id danh mục của item cần kiểm tra hay không và nếu có thì còn hoạt động hay không
			var checkedProduct = db.promotions.FirstOrDefault(m => m.category_id == item.product.category_id & m.active == true & m.start_date <= vietnamDateTime & m.end_date >= vietnamDateTime);

			// nếu không có thì trả về giá gốc
			if (checkedProduct == null){
				return (decimal)item.price;
			}
			// nếu không có thì trả về giá khuyến mãi
			else
			{
				var price = item.price - (item.price * checkedProduct.discount_rate) / 100;
				return (decimal)price;
			}
		}



		// Lấy ra thông tin khuyến mãi bằng id danh mục

		public static promotion GetPromotionDetails(int id)
		{
			Project_63130260Entities db = new Project_63130260Entities();
			DateTime vietnamDateTime = mapDateTime.GetVietnamDateTime();
			var promotion = db.promotions.FirstOrDefault(m => m.category_id == id & m.active == true & m.start_date <= vietnamDateTime & m.end_date >= vietnamDateTime);
			return promotion;
		}

		// Lấy chi tiết của khuyến mãi bằng id
		public  promotion GetPromotionDetailsById(int id)
		{
			var promotion = db.promotions.Find(id);
			return promotion;
		}

		// chỉnh sửa khuyến mãi
		public string UpdatePromotion(int id, promotion promotion)
		{
			//Kiếm tra promotion mới có danh mục chỉnh sửa trùng với các promotion khác hay không
			var checkPromotion = db.promotions.FirstOrDefault(m => m.category_id == promotion.category_id);

			var newPromotion = GetPromotionDetailsById(id);

			// nếu trùng thì báo lỗi và kết thúc
			if (checkPromotion != null & newPromotion.category_id != promotion.category_id)
			{
				return "This category already has a promotion.";
			}

			// nếu ngày kết thúc sớm hơn ngày bắt đầu thì báo lỗi và kết thúc hàm
			if (promotion.start_date > promotion.end_date)
			{
				return "The end date must be greater than the start date.";
			}

			//Nếu thỏa mãn các điều kiện thì thực hiện chỉnh sửa và lưu lại trong db
			newPromotion.name = promotion.name;
			newPromotion.category_id = promotion.category_id;
			newPromotion.description = promotion.description;
			newPromotion.start_date = promotion.start_date;
			newPromotion.end_date = promotion.end_date;
			newPromotion.discount_rate = promotion.discount_rate;
			db.SaveChanges();
			return null;
		}

		// Hủy khuyến mãi
		public void DeactivePromotion(int id)
		{
			var promotion = db.promotions.Find(id);
			if(promotion != null)
			{
				promotion.active = false;
				db.SaveChanges();
			}
		}

		// Xóa khuyến mãi
		public void DeletePromotion(int id)
		{
			var promotion = db.promotions.Find(id);
			if (promotion != null)
			{
				db.promotions.Remove(promotion);
				db.SaveChanges() ;
			}
		}

	}
}
