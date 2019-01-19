using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class FirstSubCategory
    {
        [Key]
        public int FirstSubCatId { get; set; }

        public int CatId_FK { get; set; }

        [Display(Name = "عنوان دسته")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        public string FirstSubCatTitle { get; set; }

        [Display(Name = "تصویر گروه")]
        public string Picture { get; set; }

        //-------------------------------------------------------------------

        public virtual ICollection<Products> Tbl_Products { get; set; }


    }
}


