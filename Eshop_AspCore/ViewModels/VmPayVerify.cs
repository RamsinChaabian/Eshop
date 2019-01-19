using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmPayVerify
    {
        [Required]
        public string Authority { get; set; }

        [Required]
        public string Status { get; set; } //Ok ||NOK
    }
}
