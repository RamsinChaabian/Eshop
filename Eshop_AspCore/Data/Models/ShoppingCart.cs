using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ID { get; set; }
        public int ProductId_FK { get; set; }
        public string UserId_FK { get; set; }
        public int ProductCount { get; set; }
        public DateTime DateShop { get; set; }
        public bool Status { get; set; }
        public Nullable<int> InvoiceId { get; set; }


        //-------------------------------------------------------------------

        [ForeignKey(nameof(ProductId_FK))]
        public virtual Products Tbl_Products { get; set; }

        [ForeignKey(nameof(UserId_FK))]
        public virtual ApplicationUser Tbl_User { get; set; }
    }
}
