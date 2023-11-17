using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapCategory
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra danh sách các danh mục
		public List<product_category> GetListCategory()
		{
			return db.product_category.ToList();
		}
	}
}
