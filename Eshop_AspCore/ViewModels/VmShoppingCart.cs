using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmShoppingCart
    {
        public int ID { get; set; }
        public int ProductId_FK { get; set; }
        public string UserId_FK { get; set; }
        public int ProductCount { get; set; }
        public DateTime DateShop { get; set; }
        public bool Status { get; set; }
        public string ProductImage { get; set; }
        public int OffProduct { get; set; }
        public decimal Price { get; set; }
        public decimal PriceWithOff { get; set; }
        public string ProductName { get; set; }
        public double WeightPro { get; set; }

    }
}
