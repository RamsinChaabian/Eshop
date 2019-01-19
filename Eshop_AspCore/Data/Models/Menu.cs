using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Eshop_AspCore.Data.Models
{
    public class Menu
    {

        [Key]
        public int ID { get; set; }

        [Display(Name ="نام منو")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="{0} را وارد نمایید")]
        public string Title { get; set; }

        public string Link { get; set; }

        public bool IsActive { get; set; }
    }
}
