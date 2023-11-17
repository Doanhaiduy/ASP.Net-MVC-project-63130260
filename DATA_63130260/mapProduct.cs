using DATA_63130260.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA_63130260
{

	public class mapProduct
	{
		public Project_63130260Entities db = new Project_63130260Entities();

		// Lấy ra danh sách sản phẩm hiện có trong db
		public List<product_item> getListProduct(){
			var list = db.product_item.OrderByDescending(m => m.id).Where(m => m.product.deleted == false).ToList();
			return list;
		}

		// Lấy ra chi tiết sản phẩm thông qua id
		public product_item getDetailProduct(int id)
		{
			var product = db.product_item.FirstOrDefault(m => m.product_id == id && m.product.deleted == false);
			return product;
		}

		// Lấy ra danh sách sản phẩm có danh mục bằng với id cần tìm
		public List<product_item> getListProductOfCategory(int id)
		{
			var list = db.product_item.Where(m => m.product.category_id == id && m.product.deleted == false).ToList();
			return list;
		}

		// Thêm sản phẩm vào hệ thống
		public void AddNewProduct(product product, int price, int qty_in_stock, string product_image2, product_attributes proAttr)
		{
			// tạo 1 product
			var newProduct = new product();
			newProduct.name = product.name;
			newProduct.description = product.description;
			newProduct.category_id = product.category_id;
			newProduct.product_image = product.product_image;
			newProduct.deleted = false;
			db.products.Add(newProduct);
			db.SaveChanges();

			// Lấy phần tử cuối cùng
			var lastproduct = db.products.ToList().LastOrDefault();
			// trả về product vừa thêm
			// tạo 1 product_item 
			var newProductItem = new product_item();
			newProductItem.product_id = lastproduct.id;
			newProductItem.price = price;
			newProductItem.qty_in_stock = qty_in_stock;
			newProductItem.qty_received = qty_in_stock;
			newProductItem.qty_sold = 0;
			newProductItem.product_image = product_image2;
			db.product_item.Add(newProductItem);

			// tạo 1 product_attributes
			var newProductAttr = new product_attributes();
			newProductAttr.product_id = newProduct.id;
			newProductAttr.length = proAttr.length;
			newProductAttr.width = proAttr.width;
			newProductAttr.height = proAttr.height;
			newProductAttr.weight = proAttr.weight;
			newProductAttr.materials = proAttr.materials;
			newProductAttr.other_info = proAttr.other_info;

			db.product_attributes.Add(newProductAttr);

			db.SaveChanges();
		}

		// Chỉnh sửa sản phẩm theo id
		public bool UpdateProduct(int id, product_item productItem, product product, product_attributes proAttr)
		{
			// Lấy ra sản phẩm và chi tiết sản phẩm dựa vào id truyền vào
			var newProductItem = db.product_item.Find(id);
			var newProduct = db.products.Find(newProductItem.product_id);

			if (newProductItem == null || newProduct == null)
			{
				// Nếu 1 trong 2 null thì sẽ kết thúc
				return false;
			}
			else
			{
				// Nếu lấy ra được thì chỉnh sửa lại và lưu vào db
				newProductItem.price = productItem.price;
				newProductItem.qty_in_stock = productItem.qty_in_stock;
				if(productItem.product_image == null)
				{
					newProductItem.product_image = newProductItem.product_image;
				}
				else
				{
					newProductItem.product_image = productItem.product_image;
				}

				if(product.product_image == null)
				{
					newProduct.product_image = newProduct.product_image;
				}
				else
				{
					newProduct.product_image = product.product_image;
				}
				newProduct.name = product.name;
				newProduct.description = product.description;
				newProduct.category_id = product.category_id;

				// Cập nhật cho atrributes
				var newProductAtrr = db.product_attributes.Find(newProductItem.product_id);
				newProductAtrr.length = proAttr.length;
				newProductAtrr.width = proAttr.width;
				newProductAtrr.height = proAttr.height;
				newProductAtrr.weight = proAttr.weight;
				newProductAtrr.materials = proAttr.materials;
				newProductAtrr.other_info = proAttr.other_info;
				db.SaveChanges();
				return true;
			}
		}

		//Xóa sản phẩm theo id

		public bool DeleteProduct(int id)
		{
			var product = db.products.Find(id);
			if(product == null) { return false; }
			product.deleted = true;
			db.SaveChanges();
			return true;
		}

		// Nhập thêm số lượng sản phẩm

		public bool ImportMoreProduct(int id, int qty)
		{
			var productItem = db.product_item.Find(id);
			if (productItem == null) { return false; }
			productItem.qty_in_stock +=qty;
			productItem.qty_received += qty;
			db.SaveChanges();
			return true;
		}
		// Sắp xếp danh sách sản phẩm
		public List<product_item> SortProduct(string sortBy, List<product_item> listProduct)
		{
			
				switch (sortBy)
				{
					case "best-selling":
						{
							// Sắp xếp theo qty_sold giảm dần (best-selling)
							listProduct = listProduct.OrderByDescending(p => p.qty_sold).ToList();
							break;
						}
					case "name-ascending":
						{
							// Sắp xếp theo product_id tăng dần (title-ascending)
							listProduct = listProduct.OrderBy(p => p.product.name).ToList();
							break;
						}
					case "name-descending":
						{
							// Sắp xếp theo product_id giảm dần (title-descending)
							listProduct = listProduct.OrderByDescending(p => p.product.name).ToList();
							break;
						}
					case "price-ascending":
						{
							// Sắp xếp theo price tăng dần (price-ascending)
							listProduct = listProduct.OrderBy(p => p.price).ToList();
							break;
						}
					case "price-descending":
						{
							// Sắp xếp theo price giảm dần (price-descending)
							listProduct = listProduct.OrderByDescending(p => p.price).ToList();
							break;
						}
				}
			return listProduct;
		}
	}
}
