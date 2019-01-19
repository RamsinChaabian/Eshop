using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }
        public int FileId_FK { get; set; }

        [Display(Name = "عنوان اسلایدر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string SliderTitle { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string Description { get; set; }

        [Display(Name = "لینک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} نمی تواند خالی باشد")]
        public string Link { get; set; }

        [Display(Name = "ترتیب")]
        public int Sort { get; set; }

        public bool ServerUpload { get; set; }

        //-------------------------------------------------------------------
        [ForeignKey(nameof(FileId_FK))]
        public virtual Files Tbl_Files { get; set; }


    }
}
