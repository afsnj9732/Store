using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class ShoppingCartViewModel
    {
        public int CartItemID { get; set; }
        public string ProductName { get; set; }
        public int UnitPrice {  get; set; }
        public int Quantity { get; set; }
        public int TotalPrice {  get; set; }

    }
}