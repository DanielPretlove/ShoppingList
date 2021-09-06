using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.ViewModel
{
    public class OrderDetailsViewModel
    {
       public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public string ItemID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}