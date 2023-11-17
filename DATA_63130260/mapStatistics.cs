using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapStatistics
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra thống kê của hệ thống
		public statistics GetStatisticByMonth(int month, int year)
		{
			statistics statistics = new statistics();
			// Tổng doanh thu theo tháng-năm 
			statistics.total_revenue = (float)db.shop_order.Where(m => m.order_date.Value.Month == month & m.order_date.Value.Year == year).ToList().Sum(m => m.order_total);
			// Tổng số người dùng hiện có trong hệ thống
			statistics.total_users = db.site_user.ToList().Count;
			// Tổng số người dùng mới trong tháng-năm
			statistics.new_users = db.site_user.Where(m => m.created_date.Value.Month == month & m.created_date.Value.Year == year).ToList().Count;
			// Tổng số đơn hàng đã đặt trong tháng-năm
			statistics.total_orders = db.shop_order.Where(m => m.order_date.Value.Month == month & m.order_date.Value.Year == year).ToList().Count;
			// Tổng số đơn hàng đã hoàn thành trong tháng-năm
			statistics.completed_orders = db.shop_order.Where(m => m.order_date.Value.Month == month & m.order_date.Value.Year == year & m.order_status == true).ToList().Count;
			// Tổng số sản phẩm hiện có
			statistics.total_products = db.products.ToList().Count;
			// Tổng số sản phẩm đã thêm trong tháng-năm

			// Tổng số chương trình khuyến mãi trong tháng-năm
			statistics.total_promotions = db.promotions.Where(m => (m.start_date.Value.Month == month || m.end_date.Value.Month == month) & m.start_date.Value.Year == year).ToList().Count;

			return statistics;
		}
	}
}
