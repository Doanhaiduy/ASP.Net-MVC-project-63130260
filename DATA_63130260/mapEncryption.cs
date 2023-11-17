using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public static class mapEncryption
	{
		// Mã hóa 1 chiều nhận vào chuỗi cần mã hóa, và trả về chuỗi đã được mã hóa
		public static string Encrypt(string _input)
		{
			// Mã hóa MD5: chỉ trả về 32 kí tự
			// Khởi tạo 
			using (var md5Hash = MD5.Create())
			{

				//Chuyển đổi chuỗi đầu vào thành mảng byte sử dụng UTF-8 và tính giá trị băm MD5
				byte[] data = md5Hash.ComputeHash(Encoding.Unicode.GetBytes(_input));

				//Chuyển đổi giá trị băm từ mảng byte thành chuỗi hexa và chuẩn hóa nó
				string hash = BitConverter.ToString(data).Replace("-", "").ToLower();

				// Gọi hàm MyEncrypt để thực hiện xử lý mã hóa để toàn vẹn hơn
				return MyEncrypt(hash);
			}

			// Sử dụng hàm mã hóa 2 chiều
		}

		// Mã hóa 2 chiều nhận vào chuỗi cần mã hóa, và trả về chuỗi đã được mã hóa
		public static string MyEncrypt(string _input)
		{
			// Chuyển thành mảng char
			char[] char_input = _input.ToCharArray();
			// mã hóa
			// Lấy ra từng kí tự trong mảng tương ứng với index
			var input_withIndex = char_input.Select((val, ind) => new { val, ind }).ToArray();

			// thưc hiện mã hóa theo công thức // abc => 48 49 50: 48 + 0 +(49%2); 49 + 1 + 50%2
			var char_input_encripted = input_withIndex.Select(c => c.val + c.ind + (input_withIndex.Length > c.ind + 1 ? input_withIndex[c.ind + 1].val % 2 : 0)).Select(c => (char)c).ToArray();
			
			// Chuyển về kiểu string
			string resval = new string(char_input_encripted); 
			return resval;
		}

		
	}
}
