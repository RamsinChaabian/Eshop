using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmPay
    {
        public int InvoiceId { get; set; }
        public string UserId_FK { get; set; }
        public DateTime DateOrder { get; set; }
        public bool StatusPay { get; set; }
        public decimal SumPrice { get; set; }
        public string Authority { get; set; }
        public long RefID { get; set; }
    }
}
