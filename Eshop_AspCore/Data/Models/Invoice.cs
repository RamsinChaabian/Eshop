using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public string TransactionId { get; set; }
        public string UserId_FK { get; set; }
        public DateTime DateOrder { get; set; }
        public bool StatusPay { get; set; }
        public decimal SumPrice { get; set; }
        public string Authority { get; set; }
        public long RefID { get; set; }
        public string DescriptionPay { get; set; }

        //-------------------------------------------------------------------

        [ForeignKey(nameof(UserId_FK))]
        public virtual ApplicationUser Tbl_User { get; set; }
        public virtual ICollection<ShoppingCart> Tbl_ShoppingCart { get; set; }
    }
}
