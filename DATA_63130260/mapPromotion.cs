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
		public bool AddNewPromotion(promotion promotion)
		{
			var checkPromotion = db.promotions.FirstOrDefault(m => m.category_id == promotion.category_id);
			if (checkPromotion != null)
			{
				return false;
			}
			if(promotion.start_date > promotion.end_date)
			{
				return false;
			}
			promotion.active = true;
			db.promotions.Add(promotion);
			db.SaveChanges();
			return true;
		}

		// kiểm tra và tính giá sản phẩm sau khi giảm giá 
		public static decimal CheckAndCalculatePrice(product_item item)
		{
			Project_63130260Entities db = new Project_63130260Entities();

			var checkedProduct = db.promotions.FirstOrDefault(m => m.category_id == item.product.category_id & m.active == true & m.start_date <= DateTime.Now);

			if (checkedProduct == null){
				return (decimal)item.price;
			}
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
			var promotion = db.promotions.FirstOrDefault(m => m.category_id == id & m.active == true & m.start_date <= DateTime.Now);
			return promotion;
		}

		// Lấy chi tiết của khuyến mãi bằng id
		public  promotion GetPromotionDetailsById(int id)
		{
			var promotion = db.promotions.Find(id);
			return promotion;
		}

		// chỉnh sửa khuyến mãi

		public bool UpdatePromotion(int id, promotion promotion)
		{
			var checkPromotion = db.promotions.FirstOrDefault(m => m.category_id == promotion.category_id);
			var newPromotion = GetPromotionDetailsById(id);
			if (checkPromotion != null & newPromotion.category_id != promotion.category_id)
			{
				return false;
			}
			newPromotion.name = promotion.name;
			newPromotion.category_id = promotion.category_id;
			newPromotion.description = promotion.description;
			newPromotion.start_date = promotion.start_date;
			newPromotion.end_date = promotion.end_date;
			newPromotion.discount_rate = promotion.discount_rate;
			db.SaveChanges();
			return true;
		}

		public void DeactivePromotion(int id)
		{
			var promotion = db.promotions.Find(id);
			if(promotion != null)
			{
				promotion.active = false;
				db.SaveChanges();
			}
		}
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
