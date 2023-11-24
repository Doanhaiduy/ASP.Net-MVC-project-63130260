using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{
	public static class mapSendEmail
	{

		public static void SendEmail(string receiver, shop_order order)
		{
			try
			{
				var mapOrder = new mapOrder();
				var list = mapOrder.GetOrderItems(order.id);

				var senderEmail = new MailAddress("duy.dh.63cntt@ntu.edu.vn", "Đoàn Hải Duy");
				var receiverEmail = new MailAddress(receiver, "Receiver");
				var password = "haiduy10";
				var sub = "Order Confirmation";
				var body = $@"
		<section
		    style=""display: flex; align-items: center; padding: 4rem 0; background-color: #f8f9fa; font-family: 'Poppins', sans-serif;"">
		    <div
		      style=""flex: 1; max-width: 896px ; padding: 2.5rem; margin: auto; background-color: #edf2f7; border-radius: 0.375rem; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06); padding-top: 1.5rem; padding-bottom: 1.5rem;"">

		      <div style=""margin-bottom: 2.5rem; text-align: center;"">
		        <h1
		          style=""margin-bottom: 1.5rem; font-size: 1.5rem; font-weight: 600; line-height: 1.57; color: #4a5568;"">
		          Thank you for your Order, {order.order_first_name}</h1>
		        <p style=""font-size: 1.125rem; color: #718096;"">Your order number is:
		          #{order.id}</p>
		      </div>

		      <div style=""max-width: 896px ; margin: auto; margin-bottom: 1.25rem;"">

		        <h2 style=""margin-bottom: 1rem; font-size: 1.25rem; font-weight: 500;"">
		          What you ordered:</h2>";

				foreach (var item in list)
				{

					var price = DATA_63130260.mapPromotion.CheckAndCalculatePrice(item.product_item);

					body += $@"<div

				  style =""padding: 2.5rem; margin-bottom: 2rem; background-color: #fff; border-radius: 0.375rem; box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1); display: flex; align-items: center; "">

		          <a href=""/https://project-63130260.site/Product_63130260/Details/{item.product_item.product.id}"" style=""margin-right: 1.5rem; margin-right: 3rem;"">
		            <img
		              style=""width: 100%; max-width: 80px; height: 200px; max-height: 80px; object-fit: cover; margin-bottom: 1rem;""
		              src=""{item.product_item.product.product_image}""
		              alt=""dress"">
		          </a>

		          <div>
		            <a style=""display: inline-block; margin-bottom: 0.25rem; font-size: 1.125rem; font-weight: 500; text-decoration: none; color: #1a202c; transition: color 0.2s ease-in-out;""
		              href=""#"">{item.product_item.product.name}</a>

		            <div style=""flex-wrap: wrap; display: flex;"">

		              <p
		                style=""margin-right: 1rem; font-size: 0.875rem; font-weight: 500;"">
		                <span style=""font-weight: 500;"">Price:</span>
		                <span
		                  style=""margin-left: 0.125rem; color: #a0aec0;"">{String.Format("{0:N0}đ", price)}</span>
		              </p>

		              <p
		                style=""margin-right: 1rem; font-size: 0.875rem; font-weight: 500;"">
		                <span style=""font-weight: 500;"">Size:</span>
		                <span
		                  style=""margin-left: 0.125rem; color: #a0aec0;"">medium</span>
		              </p>

		              <p style=""font-size: 0.875rem; font-weight: 500;"">
		                <span>Qty:</span>
		                <span style=""margin-left: 0.125rem; color: #a0aec0;"">{item.qty}</span>
		              </p>

		            </div>
		          </div>
		        </div> ";
			}
				body += $@"<div style=""max-width: 896px ; margin: auto;"">

		        <h2 style=""margin-bottom: 1rem; font-size: 1.25rem; font-weight: 500;"">
		          Order Details:</h2>

		        <div
		          style="" gap: 20px;display: flex;
      justify-content: space-between;
      align-items: center; margin-bottom: 2.5rem; "">

		          <div
		            style=""display: flex; align-items: center; justify-content: space-between; padding: 1rem 2.5rem; font-weight: 600; background-color: rgba(255, 255, 255, 0.5); border-radius: 0.375rem; box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);"">
		            <span>Name: </span>
		            <span style=""display: flex; align-items: center;"">
		              <span
		                style=""margin-left: 0.375rem; margin-right: 0.125rem; font-size: 0.875rem;"">{order.order_first_name} {order.order_last_name}</span>

		            </span>
		          </div>
		          <div
		            style=""display: flex; align-items: center; justify-content: space-between; padding: 1rem 2.5rem; font-weight: 600; background-color: rgba(255, 255, 255, 0.5); border-radius: 0.375rem; box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);"">
		            <span>Shipping: </span>
		            <span style=""display: flex; align-items: center;"">

		              <span style=""font-size: 0.875rem; color: #4a5568;"">{String.Format("{0:N0}đ", order.shipping_method1.price)}</span>
		            </span>
		          </div>

		          <div
		            style=""display: flex; align-items: center; justify-content: space-between; padding: 1rem 2.5rem; font-weight: 600; background-color: rgba(255, 255, 255, 0.5); border-radius: 0.375rem; box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1);"">
		            <span>Total: </span>
		            <span style=""display: flex; align-items: center;"">
		              <span style=""font-size: 0.875rem; color: #4a5568;"">{String.Format("{0:N0}đ", order.order_total)}</span>
		            </span>
		          </div>
				
		        </div>

		        <div
		          style=""flex-wrap: wrap; display: flex; align-items: center; justify-content: center; gap: 1rem;"">

		          <a href=""https://project-63130260.site/Product_63130260/""
		            style="" padding: 1rem 1.5rem; color: #4a5568; border: 1px solid #4a5568; border-radius: 0.375rem; transition: background-color 0.2s ease-in-out;""
		            class=""w-full px-6 py-3 text-blue-500 border border-blue-500 rounded-md md:w-auto hover:text-gray-100 hover:bg-blue-600"">Go
		            back shopping</a>
		        </div>
				

		      </div>
		    </div>
		  </section>";
			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(senderEmail.Address, password)
			};
			using (var mess = new MailMessage(senderEmail, receiverEmail)
			{
				Subject = sub,
				Body = body,
				IsBodyHtml = true
			})
			{
				smtp.Send(mess);
			}
		}
			catch (Exception)
			{
			}
}
	}
}
