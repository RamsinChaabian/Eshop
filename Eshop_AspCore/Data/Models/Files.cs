using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_AspCore.Data.Models
{
    public class Files
    {
        [Key]
        public int FileId { get; set; }

        public int ServerId_FK { get; set; }

        [Display(Name = "عنوان فایل")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string Title { get; set; }

        [Display(Name = "نام فیزیکی فایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}نمی تواند خالی باشد")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string FileName { get; set; }

        [Display(Name = "نام جایگزین")]
        public string Alt { get; set; }

        //-------------------------------------------------------------------

        [ForeignKey(nameof(ServerId_FK))]
        public virtual Server Tbl_Server { get; set; }
        public virtual ICollection<Slider> Tbl_Slider { get; set; }
    }
}
