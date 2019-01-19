using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Data.Models
{
    public class Weight
    {
        [Key]
        public int Weight_Id { get; set; }

        [Display(Name = "حداکثر وزن")]
        public Nullable<int> Weight_Max { get; set; }

        [Display(Name = "حداقل وزن")]
        public Nullable<int> Weight_Min { get; set; }

        [Display(Name ="هزینه")]
        public Nullable<int> Weight_Price { get; set; }

    }
}
