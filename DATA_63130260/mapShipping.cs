using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapShipping
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra danh sách đơn vị vận chuyển
		public List<shipping_method> GetListShippingType()
		{
			return db.shipping_method.ToList();
		}
		public shipping_method GetDetails(int id)
		{
			return db.shipping_method.Find(id);
		}
	}
}
