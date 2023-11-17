using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Firebase.Storage;
using Firebase.Auth;
using System.Threading;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public class mapFirebase
	{
		// Cấu hình cho Storage của firebase
		private static string ApiKey = "AIzaSyAd282MCRBP7ij6b1ECwqKCzZg0kbHjo2k";
		private static string Bucket = "project63130260.appspot.com";
		private static string AuthEmail = "admin@gmail.com";
		private static string AuthPassword = "haiduy10";

		// Thực hiện quá trình upload file lên Storage firebase
		public async Task<String> Upload(FileStream stream, string fileName)
		{
			var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
			var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

			// Có thể sử dụng CancellationTokenSource để hủy quá trình tải lên giữa chừng
			var cancellation = new CancellationTokenSource();

			// Tạo một tên tệp duy nhất bằng cách thêm một GUID ngẫu nhiên vào tên tệp
			string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

			var task = new FirebaseStorage(
				Bucket,
				new FirebaseStorageOptions
				{
					AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
					ThrowOnCancel = true // Khi hủy tải lên, ngoại lệ sẽ được đưa ra. Theo mặc định không có ngoại lệ nào được ném ra
				})
				.Child("images")
				.Child(uniqueFileName)
				.PutAsync(stream, cancellation.Token);

			try
			{
				// Lỗi trong quá trình tải lên sẽ xuất hiện khi chờ đồng bộ hóa tác vụ
				string url = await task;
				return url;
				// Trả về URL của hình ảnh vừa upload lên
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception was thrown: {0}", ex);
			}
			return uniqueFileName;
		}
	}
}
