using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class LastSubCategory
    {
        [Key]
        public int LastSubCatId { get; set; }

        public int SecondSubCatId_FK { get; set; }

        [Display(Name = "عنوان دسته")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        public string LastSubCatTitle { get; set; }

        //-------------------------------------------------------------------

        public virtual ICollection<Products> Tbl_Products { get; set; }
    }
}
