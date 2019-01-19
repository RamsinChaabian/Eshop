using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmLastSubCategory
    {
        public int LastSubCatId { get; set; }
        public int SecondSubCatId_FK { get; set; }
        public string LastSubCatTitle { get; set; }
        public string Link { get; set; }

    }
}
