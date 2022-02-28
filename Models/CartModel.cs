using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartService.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductID { set; get; } = 0;
        public double ProductPrices { set; get; } = 0.0;
    }
    public class CartModel
    {
        //[INITIATED, SUCCESS, FAILED]
        public enum EnumOrderStatus
        {
            INITIATED = 1,
            SUCCESS = 2,
            FAILED = 3
        }

        public static string GetEnumOrderStatus(int input)
        {
            return ((EnumOrderStatus)input).ToString();
        }

        [Key]
        public int CartID { set; get; } = 0;
        public int OrderStatusID { set; get; } = 0;
        public string OrderStatus
        {
            get
            {
                return GetEnumOrderStatus(OrderStatusID);
            }
        }
        public int OrderID { set; get; } = 0;
        public List<ProductModel> ProductList { set; get; } = null;
        public double Total {
            get {
                var totalPrice = 0.0;
                if (ProductList.Count > 0)
                {
                    foreach(var productModel in ProductList)
                    {
                        totalPrice = totalPrice + productModel.ProductPrices;
                    }
                }
                return totalPrice;
            }
        }
    }
}
