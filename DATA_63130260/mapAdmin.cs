using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapAdmin
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		public site_admin CheckAdminLogin(string username, string password)
		{

			// Kiểm tra xem thông tin đăng nhập có khớp với bất kì user nào không
			var admin = db.site_admin.Where(m => m.email_address == username & m.password == password).ToList();
			if (admin.Count > 0)
			{
				return admin[0];
			}
			else
			{
				return null;
			}
		}
	}
}
