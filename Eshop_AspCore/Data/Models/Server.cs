using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class Server
    {
        [Key]
        public int ServerId { get; set; }

        [Display(Name = "نام سرور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string ServerTitle { get; set; }

        [Display(Name = "آدرس سرور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        public string IP { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string FtpUsername { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد ")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        public string FtpPassword { get; set; }

        [Display(Name = "مسیر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        public string Path { get; set; }

        [Display(Name = "نوع")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        public string Type { get; set; }

        [Display(Name = "دامین")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0}را وارد نمایید")]
        [MinLength(3, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد ")]
        [RegularExpression(@"((H|http://)|(H|https://))[\w?-?\-_\./]*", ErrorMessage = "{0} را صحیح وارد نمایید")]
        public string HttpDomain { get; set; }

        //-------------------------------------------------------------------
        public ICollection<Files> Tbl_Files { get; set; }

    }
}

