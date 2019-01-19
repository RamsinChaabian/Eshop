using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmFirstSubCategory
    {
        public int FirstSubCatId { get; set; }
        public int CatId_FK { get; set; }
        public string FirstSubCatTitle { get; set; }
        public string Picture { get; set; }
        public string Link { get; set; }
        public int CountProduct { get; set; }

    }
}
