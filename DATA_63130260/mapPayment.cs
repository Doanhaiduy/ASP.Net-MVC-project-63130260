using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapPayment
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra danh sách phương thức thanh toán
		public List<payment_type> GetListPaymentType()
		{
			return db.payment_type.ToList();
		}
	}
}
