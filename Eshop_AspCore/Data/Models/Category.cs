using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_AspCore.Data.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }

        [Display(Name = "عنوان دسته")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        public string CatTitle { get; set; }

        //-------------------------------------------------------------------

        public virtual ICollection<Products> Tbl_Products { get; set; }

    }
}
