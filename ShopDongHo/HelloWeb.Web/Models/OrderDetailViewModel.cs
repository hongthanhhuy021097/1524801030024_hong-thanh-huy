using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelloWeb.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public int Price { set; get; }
    }
}