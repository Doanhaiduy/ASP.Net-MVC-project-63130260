using DATA_63130260.Entity;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{

	public class mapUser
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra danh sách tất cả người dùng trong hệ thống
		public List<site_user> GetListUser()
		{
			return db.site_user.OrderByDescending(m => m.created_date).ToList();
		}

		//Lấy thông tin người dùng
		public site_user GetDetailUser(int id)
		{
			return db.site_user.Find(id);
		}

		//Tạo mới user
		public bool CreateUser(site_user user)
		{
			// Kiểm tra xem email đã tồn tại chưa, nếu rồi thì kết thúc hàm trả về false
			if(db.site_user.Any(m => m.email_address == user.email_address))
			{
				return false;
			}
			// Nếu chưa tồn tại email thì gán các thuộc tính cho user mới
			mapCart mapCart = new mapCart();
			site_user newUser = new site_user();
			newUser.first_name = user.first_name;
			newUser.last_name = user.last_name;	
			newUser.email_address = user.email_address;
			//Mã hóa mật khẩu trước khi đưa vào csdl
			string encryptPassword = mapEncryption.Encrypt(user.password);
			newUser.password = encryptPassword;
			newUser.created_date = mapDateTime.GetVietnamDateTime();
			newUser.gender = true;
			db.site_user.Add(newUser);
			db.SaveChanges();
			//Đồng thời tạo thêm 1 giỏ hàng tương ứng với user đó
			mapCart.CreateCart(newUser.id);
			return true;
		}
		//public void dongbo()
		//{
		//	// Lấy danh sách người dùng từ cơ sở dữ liệu
		//	var users = db.site_user.ToList();

		//	foreach (var user in users)
		//	{
		//		// Thay đổi mật khẩu của người dùng
		//		var newPassword = mapEncryption.Encrypt(user.password);
		//		user.password = newPassword;
		//	}

		//	// Cập nhật dữ liệu đã thay đổi trong cơ sở dữ liệu
		//	db.SaveChanges();
		//}


		// Kiểm tra thông tin đăng nhập
		public site_user CheckUserLogin(string username, string password)
		{
			// Kiểm tra xem thông tin đăng nhập có khớp với bất kì user nào không
			// mã hóa mật khẩu để kiểm tra với csdl
			string encryptPassword = mapEncryption.Encrypt(password);
			var user = db.site_user.Where(m => m.email_address == username && m.password == encryptPassword).ToList();
			if (user.Count > 0)
			{
				return user[0];
			}
			else
			{
				return null;
			}
		}

		// Cập nhật thông tin người dùng
		public bool UpdateInfoUser(int idUser, site_user user)
		{
			// Tìm kiếm người dùng cần cập nhật theo id
			site_user newUser = db.site_user.FirstOrDefault(m => m.id == idUser);
			if(user != null) {
				newUser.first_name = user.first_name;
				newUser.last_name= user.last_name;
				newUser.email_address = user.email_address;
				newUser.phone_number = user.phone_number;
				newUser.gender = user.gender;
				db.SaveChanges();
				return true;
			}
			return false;

		}

		public int UpdateUserAddress(int idUser, site_user user, address addr, bool isOrderAdress = false )
		{
			if (user != null)
			{
				// kiểm tra xem người dùng này có địa chỉ mặc định chưa, nếu có sẽ trả về địa chỉ đầu tiên (default), còn chưa có thì trả về null
				mapAddress map = new mapAddress();
				var checkHasAddress = db.user_address.SingleOrDefault(m => m.user_id == idUser & m.is_default == true);
				// thực hiện set địa chỉ cho người dùng
				return map.SetAddressUser(addr, idUser, checkHasAddress, isOrderAdress);
			}
			return 0;
			
		}

		//Đổi mật khẩu
		public bool ChangePassword(int idUser,string currentPassword, string newPassword, string confirmPassword)
		{
			// Nếu không điền gì vào các ô mật khẩu thì sẽ không thực hiện gì hết, trả về true để thay đổi thông tin khác
			if(string.IsNullOrEmpty(currentPassword) & string.IsNullOrEmpty(newPassword) & string.IsNullOrEmpty(confirmPassword)) {
				return true;
			}
			
			site_user user = db.site_user.Find(idUser);
			if (user != null)
			{
				// Mã hóa password trước khi kiểm tra
				var currentPasswordEncrypt = mapEncryption.Encrypt(currentPassword);

				//  nếu mật khẩu hiện tại đúng và mật khẩu xác nhận đúng thì đổi mật khẩu và trả về true
				if (user.password == currentPasswordEncrypt & newPassword == confirmPassword & !String.IsNullOrEmpty(newPassword))
				{
					// Mã hóa password trước khi thay đổi
					var newPasswordEncrypt = mapEncryption.Encrypt(newPassword);
					user.password = newPasswordEncrypt;
					db.SaveChanges();
					return true;
				}
				else
				{
					// Ngược lại trả về false
					return false;
				}
			}
			return false;
		}
	}
}
