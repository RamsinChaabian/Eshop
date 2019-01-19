using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmDetailsOrder
    {
        public int ShoppingId { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int ProductCount { get; set; }
        public string ProductImage { get; set; }
        public decimal  ProductPrice { get; set; }
        public string ProductName { get; set; }
        public int OffProduct { get; set; }
    }
}
