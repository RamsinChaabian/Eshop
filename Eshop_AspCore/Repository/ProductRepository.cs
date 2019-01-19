using Eshop_AspCore.Data;
using Eshop_AspCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Repository
{
    public class ProductRepository : IDisposable
    {
        private ApplicationDbContext database = null;
        public ProductRepository()
        {
            database = new ApplicationDbContext();
        }



        public List<VmNewProduct> ShowNewProducts() //نمایش جدیدترین محصولات
        {
            var qNewPro = database.Tbl_Products.Where(c => c.IsShowProduct == true)
                                               .OrderByDescending(c => c.ProductId)
                                               .Include(c => c.Tbl_Gallery)
                                               .ToList();


            List<VmNewProduct> lstNewProduct = new List<VmNewProduct>();


            foreach (var item in qNewPro)
            {
                VmNewProduct vmNewPro = new VmNewProduct();
                vmNewPro.ProductId = item.ProductId;
                vmNewPro.ProductNameFA = item.ProductNameFA;
                vmNewPro.OffProduct = item.OffProduct;
                vmNewPro.Price = item.Price;
                vmNewPro.ProductNameEN = item.ProductNameEN;
                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                vmNewPro.DefaultPic = "/Files/Images/Products/" + productImageName;

                lstNewProduct.Add(vmNewPro);

            }

            return lstNewProduct ?? null;
        }

        public List<VmNewProduct> ShowNewProductsByCategory()
        {
            var qNewPro = database.Tbl_Products.Where(c => c.IsShowProduct == true && c.CategoryId_FK == c.Tbl_Category.CatId)
                                               .OrderByDescending(c => c.ProductId)
                                               .Include(c => c.Tbl_Gallery)
                                               .ToList();


            List<VmNewProduct> lstNewProduct = new List<VmNewProduct>();


            foreach (var item in qNewPro)
            {
                VmNewProduct vmNewPro = new VmNewProduct();
                vmNewPro.CategoryId_FK = item.CategoryId_FK;
                vmNewPro.ProductId = item.ProductId;
                vmNewPro.ProductNameFA = item.ProductNameFA;
                vmNewPro.ProductNameEN = item.ProductNameEN;
                vmNewPro.OffProduct = item.OffProduct;
                vmNewPro.Price = item.Price;
                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                vmNewPro.DefaultPic = "/Files/Images/Products/" + productImageName;

                lstNewProduct.Add(vmNewPro);

            }

            return lstNewProduct ?? null;
        }

        public int CheckExistCountProduct(int ProductId)
        {
            var q = database.Tbl_Products.Where(c => c.ProductId == ProductId).FirstOrDefault().CountProduct;
            return q;
        }

        public List<VmNewProduct> ShowRelatedProduct(int ProductId, string ProductNameFA, int LastSubCatId)
        {
            var qRelatedProduct = database.Tbl_Products.Where(c => c.IsShowProduct == true &&
                                                                   c.ProductNameFA.Contains(ProductNameFA) || c.LastSubCat_FK == LastSubCatId)
                                              .OrderByDescending(c => c.ProductId)
                                              .Include(c => c.Tbl_Gallery)
                                              .ToList();


            List<VmNewProduct> lstNewProduct = new List<VmNewProduct>();

            foreach (var item in qRelatedProduct)
            {
                VmNewProduct vmNewPro = new VmNewProduct();
                vmNewPro.ProductId = item.ProductId;
                vmNewPro.ProductNameFA = item.ProductNameFA;
                vmNewPro.OffProduct = item.OffProduct;
                vmNewPro.Price = item.Price;
                vmNewPro.ProductNameEN = item.ProductNameEN;
                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                vmNewPro.DefaultPic = "/Files/Images/Products/" + productImageName;

                lstNewProduct.Add(vmNewPro);

            }

            return lstNewProduct ?? null;
        }

        public int GetCountShoppingCartItem(string username)
        {
            var q = database.Tbl_ShoppingCart.Where(c => c.Tbl_User.UserName == username &&
                                                       c.Status == false && c.InvoiceId == null).Count();

            if (q == 0)
                return 0;
            else
                return q;

        }

        ~ProductRepository()
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
