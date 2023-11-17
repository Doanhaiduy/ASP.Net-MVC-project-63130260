using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapAddress
	{
		public Project_63130260Entities db = new Project_63130260Entities();
		//public address GetAddress(int idUser)
		//{
		//	return db.addresses.Where(m => m.id == )
		//}

		public address CreateNewAddress(address addr, user_address checkHasAddress, int idUser,bool isOrderAdress = false)
		{
			//var checkAddress = db.addresses.SingleOrDefault(m => m.unit_number == addr.unit_number & m.district == addr.district & m.city == addr.city & m.province == addr.province & m.postal_code == addr.postal_code);
			// nếu địa chỉ này chưa tồn tại (vì check ở hàm trước ra bằng null) thì sẽ thực hiện thêm 1 địa chỉ mới vào bảng address
			if (checkHasAddress == null || isOrderAdress == true)
			{
				
				address newAddress = new address();

				newAddress.street_number = addr.street_number;
				newAddress.district = addr.district;
				newAddress.city = addr.city;
				newAddress.province = addr.province;
				newAddress.postal_code = addr.postal_code;
				db.addresses.Add(newAddress);
				db.SaveChanges();

				// Lấy phần tử cuối cùng
				var lastAddress = db.addresses.ToList().LastOrDefault();
				// trả về địa chỉ vừa thêm

				if (isOrderAdress == true && checkHasAddress != null)
				{
					// Biến isOrderAdress để kiếm tra xem có phải địa chỉ được nhập khi đặt hàng hay không
					// Nếu là địa chỉ khi đặt hàng thì ta sẽ tạo thêm 1 quan hệ mới cho địa chỉ này và người dùng, nhưng sẽ để default là fasle (vì đã có 1 địa chỉ mặc định rồi)
					user_address newOrderAddress = new user_address();
					newOrderAddress.user_id = idUser;
					newOrderAddress.address_id = lastAddress.id;
					newOrderAddress.is_default = false;
					db.user_address.Add(newOrderAddress);
					db.SaveChanges();
				}
				return lastAddress;
			}
			else
			{
					// Còn các trường hợp còn lại sẽ không xử lý nên tới đây return kết thúc
					// nếu địa chỉ đã tồn tại thì chỉ thực hiện việc update cho địa chỉ đó và lưu lại vào db
					address newAddress = db.addresses.Find(addr.id);
					newAddress.street_number = addr.street_number;
					newAddress.district = addr.district;
					newAddress.city = addr.city;
					newAddress.province = addr.province;
					newAddress.postal_code = addr.postal_code;
					db.SaveChanges();
					return newAddress;
				
			}
		}

		public int SetAddressUser(address addr, int idUser, user_address checkHasAddress, bool isOrderAdress = false)
		{
			// tạo địa chỉ vừa truyền vào, kèm theo 1 biến để check xem địa chỉ này tôn tại chưa
			var newAddress = CreateNewAddress(addr, checkHasAddress, idUser, isOrderAdress);
			// nếu địa chỉ đã tồn tại thì chỉ thực hiện việc update cho địa chỉ đó thôi
			if(checkHasAddress != null)
			{
					return newAddress.id;
			}
			// Nếu địa chỉ này chưa tồn tại thì đã tạo ở trên, và thêm 1 mối quan hệ vào bảng user_address và lưu lại db
			user_address newUserAddress = new user_address();
			newUserAddress.user_id = idUser;
			newUserAddress.address_id = newAddress.id;
			newUserAddress.is_default = true;
			db.user_address.Add(newUserAddress);
			db.SaveChanges();
			return newAddress.id;
		}

		public void SetDefaultAddressUser(int idUser, int idAddress)
		{
			var addressUser = db.user_address.FirstOrDefault(m => m.user_id == idUser && m.address_id == idAddress);
			if(addressUser != null)
			{
				var UserAddressCurrDefault = db.user_address.FirstOrDefault(m => m.user_id == idUser && m.is_default == true);
				if(UserAddressCurrDefault != null)
				{
					UserAddressCurrDefault.is_default = false;
					db.SaveChanges();
				}
				addressUser.is_default = true;
				db.SaveChanges();
			}
		}
	}
}
