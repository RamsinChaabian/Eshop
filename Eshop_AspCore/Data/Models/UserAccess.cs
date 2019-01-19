using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class UserAccess
    {
        [Key]
        public int ID { get; set; }

        public string UserId_FK { get; set; }
        public int RoleId_FK { get; set; }

        [Display(Name ="اضافه کردن")]
        public bool AddPost { get; set; }

        [Display(Name ="حذف کردن")]
        public bool DeletePost { get; set; }

        [Display(Name ="ویرایش کردن")]
        public bool EditPost { get; set; }

        [ForeignKey(nameof(RoleId_FK))]
        public virtual Role Tbl_Role { get; set; }

        [ForeignKey(nameof(UserId_FK))]
        public virtual ApplicationUser Tbl_User { get; set; }
    }
}
