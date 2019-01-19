using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_AspCore.Data.Models
{
    public class Gallery
    {
        [Key]
        public int PictureId { get; set; }

        public int ProductId_FK { get; set; }

        [Display(Name = "عنوان تصویر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string TitlePicture { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "نام عکس نمی تواند خالی باشد")]
        public string PictureName { get; set; }

        [Display(Name = "تصویر پیش فرض")]
        public bool DefaultPicProduct { get; set; }

        //-------------------------------------------------------------------
        [ForeignKey(nameof(ProductId_FK))]
        public virtual Products Tbl_Products { get; set; }


    }
}
