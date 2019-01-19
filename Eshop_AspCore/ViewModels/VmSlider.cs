using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.ViewModels
{
    public class VmSlider
    {
        public int SliderId { get; set; }
        public int FileId_FK { get; set; }
        public string SliderTitle { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int Sort { get; set; }
        public string ImageUrl { get; set; }
    }
}
