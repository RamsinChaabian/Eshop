using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmNewProduct
    {
        public int ProductId { get; set; }  
        public string ProductNameFA { get; set; }
        public string ProductNameEN { get; set; }
        public string DefaultPic { get; set; }
        public decimal Price { get; set; }
        public int OffProduct { get; set; }
        public int CategoryId_FK { get; set; }
    }
}
