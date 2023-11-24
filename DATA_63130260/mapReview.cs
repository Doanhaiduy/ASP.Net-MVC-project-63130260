using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapReview
	{

		public Project_63130260Entities db = new Project_63130260Entities();

		// Kiểm tra xem sản phẩm này đã được đánh giá chưa
		public bool CheckReviewedProduct(int id)
		{
			var product = db.order_line.Find(id);
			return (bool)product.reviewed;
		}

		// Thêm đánh giá mới

		public void AddNewReviewProduct(user_review userReview)
		{
			var newReview = new user_review();
			newReview.user_id = userReview.user_id;
			newReview.ordered_product_id = userReview.ordered_product_id;
			newReview.rating_value = userReview.rating_value;
			newReview.rating_date = mapDateTime.GetVietnamDateTime();
			newReview.comment = userReview.comment;
			var orderLine = db.order_line.Find(userReview.ordered_product_id);
			orderLine.reviewed = true;
			db.user_review.Add(newReview);
			db.SaveChanges();
		}

		// Lấy ra các lượt đánh giá của sản phẩm theo id sản phẩm
		public List<user_review> GetAllReviews(int id)
		{
			var list = db.user_review.Where(m => m.order_line.product_item_id == id).ToList();
			return list;
			
		}

		// Lấy ra các lượt đánh giá của sản phẩm theo id sản phẩm
		public double GetAvgRatingProduct(int id)
		{
			var list = db.user_review.Where(m => m.order_line.product_item_id == id).ToList().Average(m => m.rating_value);
			if(list != null)
			{
			return (double)list;

			}
			else
			{
				return 0;
			}

		}
	}
}
