using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop_AspCore.Data.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }
        public string UserId_FK { get; set; }

        [Display(Name = "کد محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string ProductCode { get; set; }

        [Display(Name = "دسته اصلی محصول")]
        public int CategoryId_FK { get; set; }

        [Display(Name = "زیر دسته اول محصول")]
        public int FirstSubCat_FK { get; set; }

        [Display(Name = "زیر دسته دوم محصول")]
        public int SecondSubCat_FK { get; set; }

        [Display(Name = "آخرین زیر دسته محصول")]
        public int LastSubCat_FK { get; set; }

        [Display(Name = "نام فارسی محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "نمی تواند بیش از 100 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "نمی تواند کمتر از 3 کاراکتر باشد")]
        public string ProductNameFA { get; set; }

        [Display(Name = "نام لاتین محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [MaxLength(100, ErrorMessage = "نمی تواند بیش از 100 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "نمی تواند کمتر از 3 کاراکتر باشد")]
        public string ProductNameEN { get; set; }

        [Display(Name = "تصویر شاخص")]
        public string DefaultPic { get; set; }

        [Display(Name = "تعداد بازدید محصول")]
        public int SeeProduct { get; set; }

        [Display(Name = "مبلغ محصول")]
        public decimal Price { get; set; }

        [Display(Name = "نمایش محصول")]
        public bool IsShowProduct { get; set; }

        [Display(Name = "میزان تخفیف محصول")]
        public int OffProduct { get; set; }

        [Display(Name = "تعداد موجود")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int CountProduct { get; set; }

        [Display(Name = "تاریخ ثبت محصول")]
        public DateTime DateProduct { get; set; }

        [Display(Name = "توضیحات محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "وزن محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int Weight { get; set; }


        //-------------------------------------------------------------------
        [ForeignKey(nameof(UserId_FK))]
        public virtual ApplicationUser Tbl_User { get; set; }

        [ForeignKey(nameof(CategoryId_FK))]
        public virtual Category Tbl_Category { get; set; }

        [ForeignKey(nameof(FirstSubCat_FK))]
        public virtual FirstSubCategory Tbl_FirstSubCategory { get; set; }

        [ForeignKey(nameof(SecondSubCat_FK))]
        public virtual SecondSubCategory Tbl_SecondSubCategory { get; set; }

        [ForeignKey(nameof(LastSubCat_FK))]
        public virtual LastSubCategory Tbl_LastSubCategory { get; set; }

        public virtual ICollection<Comments> Tbl_Comments { get; set; }

        public virtual ICollection<Gallery> Tbl_Gallery { get; set; }

        public virtual ICollection<ShoppingCart> Tbl_ShoppingCart { get; set; }
    }
}
