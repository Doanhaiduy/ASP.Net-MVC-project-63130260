using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_63130260.VNPay
{
	public class OrderDataModel
	{
		public shop_order order;
		public address other_address;
		public string shipping_address;
		public bool address_default;
		public int id_address_default;
		public decimal totalOrder = 0;
		public int shipping_method = 1;
	}
}