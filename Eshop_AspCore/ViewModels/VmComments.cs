using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmComments
    {
        public int CommentId { get; set; }
        public int ProductId_FK { get; set; }
        public string UserId_FK { get; set; }
        public string ProductName { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateComment { get; set; }
        public bool ConfirmComment { get; set; }
    }
}
