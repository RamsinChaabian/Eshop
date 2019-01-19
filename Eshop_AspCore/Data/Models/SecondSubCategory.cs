using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class SecondSubCategory
    {
        [Key]
        public int SecondSubCatId { get; set; }

        public int FirstSubCatId_FK { get; set; }

        [Display(Name = "عنوان دسته")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        public string SecondSubCatTitle { get; set; }

        //-------------------------------------------------------------------

        public virtual ICollection<Products> Tbl_Products { get; set; }

    }
}
