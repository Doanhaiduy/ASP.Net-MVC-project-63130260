using DATA_63130260;
using DATA_63130260.Entity;
using Org.BouncyCastle.Asn1.X509;
using PayPal.Api;
using Project_63130260.App_Start;
using Project_63130260.Models;
using QLBH_AnkerShop.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_63130260.Controllers
{
	public class Home_63130260Controller : Controller
	{

		site_user user = SessionConfig.GetUser(); // Lấy ra user của session hiện tại
		public ActionResult Index()
		{
			mapUser mapUser = new mapUser();
			//mapUser.dongbo();
			// Tạo session để lưu lại sản phẩm trong mục so sánh
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item>;

			if (comparedProducts == null)
			{
				// Nếu session chưa tồn tại, tạo một danh sách trống.
				comparedProducts = new List<product_item>();
				Session["ComparedProducts"] = comparedProducts;
			}
			mapProduct map = new mapProduct();
			return View(map.getListProduct());
		}

		[RoleUser]
		public ActionResult Cart()
		{
			mapCart map = new mapCart();
			var cart = map.GetCart(user.id);
			var listCartItems = map.GetCartItems(cart.id);
			return View(listCartItems);
		}

		[RoleUser]
		[HttpPost]
		public ActionResult AddProductToCart(int idProduct, int qty = 1)
		{
			mapCart map = new mapCart();
			map.AddNewCartItem(user.id, idProduct, qty);
			return RedirectToAction("Cart");
		}

		[RoleUser]
		[HttpPost]
		public ActionResult DeleteProductCart(int idProduct)
		{
			mapCart map = new mapCart();
			map.DeleteCartItem(map.GetCart(user.id).id, idProduct);
			return RedirectToAction("Cart");
		}

		[RoleUser]
		public ActionResult Checkout()
		{
			return View();
		}

		[RoleUser]
		[HttpPost]
		public async Task<ActionResult> Checkout(shop_order order, address other_address, string shipping_address, bool address_default, int id_address_default = 0, int idCart = 0, decimal totalOrder = 0, int shipping_method = 1 )
		{
			mapOrder map = new mapOrder();
			mapUser mapUser = new mapUser();
			order.user_id = user.id;
			int idAddressOrder = 0;
			// Nếu chọn địa chỉ mặc định thì lấy địa chỉ của người dùng hiện tại
			if (shipping_address == "default")
			{
				if (address_default == true)
				{
					// Lấy ra địa chỉ mặc định của người dùng hiện tại
					order.shipping_address = mapUser.GetDetailUser(user.id).user_address.FirstOrDefault(m => m.user_id == user.id && m.is_default == true).address_id;
				}
				else
				{
					order.shipping_address = mapUser.GetDetailUser(user.id).user_address.FirstOrDefault(m => m.user_id == user.id && m.is_default == false && m.address_id == id_address_default).address_id;
				}
			}
			else
			{
				//Nếu chọn địa chỉ mới thì sẽ tạo 1 địa chỉ mới cho người dùng
				//(nếu người dùng chưa cập nhật địa chỉ ở thông tin cá nhân thì địa chỉ này cũng sẽ là địa chỉ mặc định
				//tham số thứ 4 (true) để xác định đây là địa chỉ được gửi đi lúc checkout
				idAddressOrder = mapUser.UpdateUserAddress(user.id, user, other_address, true);
			}
			var Order = await map.AddNewOrder(idCart, order, idAddressOrder, totalOrder);
			var Orderid = Order.id;
			return Redirect("/home_63130260/OrderSuccess/" + Orderid);
		}

		[RoleUser]

		public async Task<ActionResult> PaymentWithPaypal(shop_order order, address other_address, string shipping_address,bool address_default, int id_address_default = 0, int idCart = 0, decimal totalOrder = 0, int shipping_method = 1, string Cancel = null)
		{
			//getting the apiContext  
			APIContext apiContext = PaypalConfiguration.GetAPIContext();
		 	try
			{
				//A resource representing a Payer that funds a payment Payment Method as paypal  
				//Payer Id will be returned when payment proceeds or click to pay  
				string payerId = Request.Params["PayerID"];
				if (string.IsNullOrEmpty(payerId))
				{
					//this section will be executed first because PayerID doesn't exist  
					//it is returned by the create function call of the payment class  
					// Creating a payment  
					// baseURL is the url on which paypal sendsback the data.  
					string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/home_63130260/PaymentWithPayPal?";
					//here we are generating guid for storing the paymentID received in session  
					//which will be used in the payment execution  
					var guid = Convert.ToString((new Random()).Next(100000));
					//CreatePayment function gives us the payment approval url  
					//on which payer is redirected for paypal account payment  
					var createdPayment = this.CreatePayment(apiContext,
						baseURI + "guid=" + guid
						+ "&order_first_name=" + order.order_first_name
						+ "&order_last_name=" + order.order_last_name
						+ "&order_phone=" + order.order_phone
						+ "&shipping_address=" + shipping_address
						+ "&street_number=" + other_address.street_number
						+ "&district=" + other_address.district
						+ "&city=" + other_address.city
						+ "&province=" + other_address.province
						+ "&postal_code=" + other_address.postal_code
						+ "&address_default="+ address_default
						+ "&id_address_default=" + id_address_default
						+ "&shipping_method=" + order.shipping_method
						+ "&payment_method_id=" + order.payment_method_id
						+ "&order_note=" + order.order_note
						+ "&totalOrder=" + totalOrder
						+ "&idCart=" + idCart,
						shipping_method);
					//get links returned from paypal in response to Create function call  
					var links = createdPayment.links.GetEnumerator();
					string paypalRedirectUrl = null;
					while (links.MoveNext())
					{
						Links lnk = links.Current;
						if (lnk.rel.ToLower().Trim().Equals("approval_url"))
						{
							//saving the payapalredirect URL to which user will be redirected for payment  
							paypalRedirectUrl = lnk.href;
						}
					}
					// saving the paymentID in the key guid  
					Session.Add(guid, createdPayment.id);
					return Redirect(paypalRedirectUrl);
				}
				else
				{
					// This function exectues after receving all parameters for the payment  

					var guid = Request.Params["guid"];
					var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

					//If executed payment failed then we will show payment failure message to user  
					if (executedPayment.state.ToLower() != "approved")
					{
						return View("FailureView");
					}

					var data = new
					{
						order_first_name = Request.QueryString["order_first_name"],
						order_last_name = Request.QueryString["order_last_name"],
						order_phone = Request.QueryString["order_phone"],
						shipping_address = Request.QueryString["shipping_address"],
						street_number = Request.QueryString["street_number"],
						district = Request.QueryString["district"],
						city = Request.QueryString["city"],
						province = Request.QueryString["province"],
						postal_code = Request.QueryString["postal_code"],
						shipping_method = Request.QueryString["shipping_method"],
						address_default = Request.QueryString["address_default"],
						id_address_default = Request.QueryString["id_address_default"],
						payment_method_id = Request.QueryString["payment_method_id"],
						order_note = Request.QueryString["order_note"],
						totalOrder = Request.QueryString["totalOrder"],
						idCart = Request.QueryString["idCart"],
					};

					int.TryParse(data.payment_method_id, out int paymentMethodId);
					int.TryParse(data.shipping_method, out int shippingMethod);
					int.TryParse(data.totalOrder, out int TotalOrder);
					int.TryParse(data.id_address_default, out int idAddressDefault);
					bool.TryParse(data.address_default, out bool addressDefault);

					mapOrder map = new mapOrder();
					mapUser mapUser = new mapUser();
					shop_order newOrder = new shop_order();
					address newAddress = new address();
					newOrder.user_id = user.id;
					newOrder.order_first_name = data.order_first_name;
					newOrder.order_last_name = data.order_last_name;
					newOrder.order_phone = data.order_phone;
					newOrder.payment_method_id = paymentMethodId;
					newOrder.shipping_method = shippingMethod;
					newOrder.order_note = data.order_note;

					newAddress.street_number = data.street_number;
					newAddress.district = data.district;
					newAddress.city = data.city;
					newAddress.province = data.province;
					newAddress.postal_code = data.postal_code;
					totalOrder = TotalOrder;
					int idAddressOrder = 0;

					// Nếu chọn địa chỉ mặc định thì lấy địa chỉ của người dùng hiện tại
					if (data.shipping_address == "default")
					{
						if(address_default == true)
						{
						// Lấy ra địa chỉ mặc định của người dùng hiện tại
						newOrder.shipping_address = mapUser.GetDetailUser(user.id).user_address.FirstOrDefault(m => m.user_id == user.id && m.is_default == true).address_id;
						}
						else
						{
							newOrder.shipping_address = mapUser.GetDetailUser(user.id).user_address.FirstOrDefault(m => m.user_id == user.id && m.is_default == false && m.address_id == idAddressDefault).address_id;
						}

					}
					else
					{
						// Nếu chọn địa chỉ mới thì sẽ tạo 1 địa chỉ mới cho người dùng
						// (nếu người dùng chưa cập nhật địa chỉ ở thông tin cá nhân thì địa chỉ này cũng sẽ là địa chỉ mặc định )
						// tham số thứ 4 (true) để xác định đây là địa chỉ được gửi đi lúc checkout
						idAddressOrder = mapUser.UpdateUserAddress(user.id, user, newAddress, true);
					}

					var Order = await map.AddNewOrder(idCart, newOrder, idAddressOrder, totalOrder);
					var Orderid = Order.id;
					return Redirect("/home_63130260/OrderSuccess/"+ Orderid);
				}
			}
			catch (Exception ex)
			{
				return View("FailureView");
			}
			//on successful payment, show success page to user.  
		}

		private PayPal.Api.Payment payment;
		[RoleUser]

		private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
		{
			var paymentExecution = new PaymentExecution()
			{
				payer_id = payerId
			};
			this.payment = new Payment()
			{
				id = paymentId
			};
			return this.payment.Execute(apiContext, paymentExecution);
		}
		[RoleUser]

		private Payment CreatePayment(APIContext apiContext, string redirectUrl, int shipping_method = 1)
		{
			//create itemlist and add item objects to it  
			var itemList = new ItemList()
			{
				items = new List<Item>()
			};

			mapShipping mapShipping = new mapShipping();

			mapCart map = new mapCart();
			mapPromotion mapPromotion = new mapPromotion();
			var cart = map.GetCart(user.id);
			var listCartItems = map.GetCartItems(cart.id);

			//Adding Item Details like name, currency, price etc  
			double test = map.GetToTalCart(cart.id);
			double totalOrder = 0;
			double totalShipping = Math.Round((double)mapShipping.GetDetails(shipping_method).price / 24275, 2);

			foreach (var item in listCartItems)
			{
				var price = mapPromotion.CheckAndCalculatePrice(item.product_item);
				itemList.items.Add(new Item()
				{
					name = item.product_item.product.name,
					currency = "USD",
					price = Math.Round((double)price / 24275, 2).ToString(),
					quantity = item.qty.ToString(),
					sku = item.product_item.product_id.ToString(),
				});
				totalOrder += Math.Round((double)price / 24275, 2) * (double)item.qty;
			}



			//Adding Item Details like name, currency, price etc  

			var payer = new Payer()
			{
				payment_method = "paypal"
			};
			// Configure Redirect Urls here with RedirectUrls object  
			var redirUrls = new RedirectUrls()
			{
				cancel_url = redirectUrl.Replace(" ","+") + "&Cancel=true",
				return_url = redirectUrl.Replace(" ", "+")
			};
			// Adding Tax, shipping and Subtotal details  
			var details = new Details()
			{
				tax = "0",
				shipping = totalShipping.ToString(),
				subtotal = totalOrder.ToString(),
			};
			//Final amount with details  
			var amount = new Amount()
			{
				currency = "USD",
				total = (totalOrder + totalShipping).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
				details = details
			};
			var transactionList = new List<Transaction>();
			// Adding description about the transaction  
			var paypalOrderId = DateTime.Now.Ticks;
			transactionList.Add(new Transaction()
			{
				description = $"Invoice #{paypalOrderId}",
				invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
				amount = amount,
				item_list = itemList
			});
			this.payment = new Payment()
			{
				intent = "sale",
				payer = payer,
				transactions = transactionList,
				redirect_urls = redirUrls
			};
			// Create a payment using a APIContext  
			return this.payment.Create(apiContext);
		}

		[RoleUser]


		public ActionResult FailureView()
		{
			return View();
		}

		[RoleUser]

		public ActionResult OrderSuccess( int id)
		{
			ViewBag.id = id;
			return View();
		}

		[RoleUser]
		public ActionResult DetailsOrder(int id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			mapOrder map = new mapOrder();
			var order = map.GetDetailsOrder(id);
			if (order == null)
			{
				return HttpNotFound();
			}
			return View(order);
		}

		[RoleUser]
		public ActionResult Whish()
		{

			return View();
		}

		public ActionResult Compare()
		{
			// Lấy danh sách sản phẩm so sánh từ Session (nếu đã tồn tại).
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item> ?? new List<product_item>();
			return View(comparedProducts);
		}

		public ActionResult AddToCompare(int id)
		{
			// Lấy danh sách sản phẩm so sánh từ Session (nếu đã tồn tại).
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item> ?? new List<product_item>();

			// Tìm sản phẩm theo ID trong cơ sở dữ liệu và thêm vào danh sách so sánh.
			mapProduct map = new mapProduct();
			var productCompare = map.getDetailProduct(id);
			if (productCompare != null)
			{
				// Chỉ giới hạn so sánh 2 sản phẩm, nếu đã có 2 sản phẩm trong mục so sánh thì sẽ thay thế sản phẩm này bằng sản phẩm đầu
				if (comparedProducts.Count == 2)
				{
					comparedProducts.Remove(comparedProducts[0]);
				}
				comparedProducts.Add(productCompare);
				// Lưu danh sách sản phẩm so sánh vào Session.
				Session["ComparedProducts"] = comparedProducts;
			}
			return RedirectToAction("Compare");
		}

		// Xóa sản phẩm so sánh
		public ActionResult RemoveFromCompare(int id)
		{
			List<product_item> comparedProducts = Session["ComparedProducts"] as List<product_item>;

			if (comparedProducts != null)
			{
				product_item product = comparedProducts.FirstOrDefault(p => p.id == id);

				if (product != null)
				{
					comparedProducts.Remove(product);
					Session["ComparedProducts"] = comparedProducts;
				}
			}

			return RedirectToAction("Compare");
		}
	}
}