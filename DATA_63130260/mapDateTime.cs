using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public static class mapDateTime
	{
		public static DateTime GetVietnamDateTime()
		{
			// Lấy múi giờ của Việt Nam
			TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

			// Lấy ngày giờ hiện tại theo múi giờ UTC
			DateTime utcNow = DateTime.UtcNow;

			// Chuyển đổi ngày giờ UTC sang múi giờ của Việt Nam
			DateTime vietnamDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, vietnamTimeZone);

			return vietnamDateTime;
		}
	}
}
