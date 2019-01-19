using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Eshop_AspCore.Data.Models
{
    public class ApplicationUser :IdentityUser

    {
        [Display(Name ="نام")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="{0} خود را وارد کنید ")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} خود را وارد کنید ")]
        public string LastName { get; set; }

        [Display(Name = "استان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} خود را وارد کنید ")]
        public string Province { get; set; }

        [Display(Name = "شهر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} خود را وارد کنید ")]
        public string City { get; set; }

        [Display(Name = "موبایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} خود را وارد کنید ")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Display(Name = "آدرس")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} خود را وارد کنید ")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "کدپستی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} خود را وارد کنید ")]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

    }
}
