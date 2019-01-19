using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmSecondSubCategory
    {
        public int SecondSubCatId { get; set; }
        public int FirstSubCatId_FK { get; set; }
        public string SecondSubCatTitle { get; set; }
        public string Link { get; set; }

    }
}
