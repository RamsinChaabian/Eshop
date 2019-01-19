using Eshop_AspCore.Data;
using Eshop_AspCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Repository
{
    public class SliderRepository : IDisposable
    {
        private ApplicationDbContext database = null;
        public SliderRepository()
        {

            database = new ApplicationDbContext();
        }

        public List<VmSlider> ShowSlider()
        {
            List<VmSlider> lstSlide = new List<VmSlider>();

            var qSlider = database.Tbl_Slider.OrderBy(c => c.Sort)
                .Include(c => c.Tbl_Files)
                .ThenInclude(c => c.Tbl_Server)
                .ToList();

            if (qSlider == null)
                return null;
            foreach (var item in qSlider)//http://wwww.Website.com/Slider-image/file.jpg
            {
                if (item.ServerUpload == true)
                {
                    VmSlider vmSlider = new VmSlider();
                    vmSlider.SliderId = item.SliderId;
                    vmSlider.FileId_FK = item.FileId_FK;
                    vmSlider.SliderTitle = item.SliderTitle;
                    vmSlider.Description = item.Description;
                    vmSlider.Link = item.Link;
                    vmSlider.ImageUrl = item.Tbl_Files.Tbl_Server.HttpDomain.TrimEnd(new char[] { '/' }) + "/" + item.Tbl_Files.FileName;

                    lstSlide.Add(vmSlider);
                }
                else
                {
                    VmSlider vmSlider = new VmSlider();
                    vmSlider.SliderId = item.SliderId;
                    vmSlider.FileId_FK = item.FileId_FK;
                    vmSlider.SliderTitle = item.SliderTitle;
                    vmSlider.Description = item.Description;
                    vmSlider.Link = item.Link;
                    vmSlider.ImageUrl = "/Files/Images/Slider/" + item.Tbl_Files.FileName;

                    lstSlide.Add(vmSlider);

                }
            }



            return lstSlide ?? null;
        }

        ~SliderRepository()
        {
            Dispose(true);
        }
        public void Dispose()
        {

        }

        public void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                Dispose();
            }
        }

    }
}
