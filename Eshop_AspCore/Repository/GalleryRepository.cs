using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop_AspCore.Data;
using Eshop_AspCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Eshop_AspCore.ViewModels.VmDetailsProduct;

namespace Eshop_AspCore.Repository
{
    public class GalleryRepository : IDisposable
    {
        ApplicationDbContext database = null;
        public GalleryRepository()
        {
            database = new ApplicationDbContext();
        }


        public List<ProductGallery> GetGalleryProduct(int ProductId)
        {
            var qGallery = database.Tbl_Gallery.Where(c => c.ProductId_FK == ProductId)
                                             .OrderBy(c => c.DefaultPicProduct)
                                             .Include(c => c.Tbl_Products)
                                             .ToList();

            if (qGallery == null)
                return null;


            List<ProductGallery> lstGallery = new List<ProductGallery>();

            foreach (var item in qGallery)
            {
                if (item != null)
                {
                    ProductGallery vm = new ProductGallery(); // => Is ViewModel

                    vm.DefaultPicProduct = item.DefaultPicProduct;
                    vm.PictureId = item.PictureId;
                    vm.PictureName = item.PictureName;
                    vm.ProductId_FK = item.ProductId_FK;
                    vm.TitlePicture = item.TitlePicture;
                    lstGallery.Add(vm);
                }
            }

            return lstGallery ?? null;
        }






        ~GalleryRepository()
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
