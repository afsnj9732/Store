using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class ShoppingCartViewModel
    {
        public int CartItemID { get; set; }

        [DisplayName("商品名稱")]
        public string ProductName { get; set; }

        [DisplayName("商品單價")]
        public int UnitPrice {  get; set; }
        [DisplayName("數量")]
        public int Quantity { get; set; }
        public int TotalPrice {  get; set; }
        public int CartID {  get; set; }

    }
}