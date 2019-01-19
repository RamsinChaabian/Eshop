using Eshop_AspCore.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Eshop_AspCore.Data;
using Microsoft.EntityFrameworkCore;
using Eshop_AspCore.ViewModels;
using Eshop_AspCore.Data.Models;

namespace Eshop_AspCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            MenuRepository menuRepository = new MenuRepository();

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Search(string FirstCatId, decimal minPrice = 0, decimal maxPrice = 0, string txtSearch = "")
        {
            string id = "";
            string title = "";
            List<string> lstCatId = new List<string>();
            List<VmProductsByCategory> lstCatProducts = new List<VmProductsByCategory>();
            if (FirstCatId != null && FirstCatId != "" && FirstCatId != string.Empty)
            {
                ViewBag.BackUrl = "/Home/Search?FirstCatId=" + FirstCatId;
                var category = FirstCatId.Trim().Split('-');
                foreach (var item in category)
                {
                    string name = item.Trim();
                    lstCatId.Add(name);
                }
                id = lstCatId.FirstOrDefault();
                title = lstCatId.LastOrDefault();
                int? ID = Convert.ToInt32(id);

                ApplicationDbContext database = new ApplicationDbContext();

                var qCategory = database.Tbl_FirstSubCategory.Where(c => c.FirstSubCatTitle.Equals(title)
                                                                    && c.FirstSubCatId == ID)
                                                                    .FirstOrDefault();

                if (qCategory == null)
                    return Redirect(nameof(Error));

                var qProduct = database.Tbl_Products.Where(c => c.FirstSubCat_FK == qCategory.FirstSubCatId
                                                     && c.IsShowProduct == true && c.FirstSubCat_FK == ID)
                                                     .Include(c => c.Tbl_Gallery)
                                                     .OrderByDescending(c => c.ProductId);


                if (minPrice == 0 && maxPrice == 0)
                {
                    if (txtSearch != null && txtSearch != "")
                    {
                        foreach (var item in qProduct.Where(c => c.ProductNameFA.Contains(txtSearch) || c.ProductNameEN.Contains(txtSearch)))
                        {
                            if (qCategory != null)
                            {
                                VmProductsByCategory vm = new VmProductsByCategory();
                                vm.OffProduct = item.OffProduct;
                                vm.Price = item.Price;
                                vm.ProductId = item.ProductId;
                                vm.ProductNameEN = item.ProductNameEN;
                                vm.ProductNameFA = item.ProductNameFA;
                                vm.SeeProduct = item.SeeProduct;
                                vm.FirstSubCat_FK = item.FirstSubCat_FK;
                                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                                vm.DefaultPic = "/Files/Images/Products/" + productImageName;
                                vm.TitleFirstCat = qCategory.FirstSubCatTitle;
                                ViewBag.SearchNamePro = txtSearch;
                                lstCatProducts.Add(vm);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in qProduct)
                        {
                            if (qCategory != null)
                            {
                                VmProductsByCategory vm = new VmProductsByCategory();
                                vm.OffProduct = item.OffProduct;
                                vm.Price = item.Price;
                                vm.ProductId = item.ProductId;
                                vm.ProductNameEN = item.ProductNameEN;
                                vm.ProductNameFA = item.ProductNameFA;
                                vm.SeeProduct = item.SeeProduct;
                                vm.FirstSubCat_FK = item.FirstSubCat_FK;
                                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                                vm.DefaultPic = "/Files/Images/Products/" + productImageName;
                                vm.TitleFirstCat = qCategory.FirstSubCatTitle;

                                lstCatProducts.Add(vm);
                            }
                        }
                    }

                }
                else // جست و جو بر اساس قیمت
                {
                    if (txtSearch != null && txtSearch != "")
                    {
                        foreach (var item in qProduct.Where(c => (c.ProductNameFA.Contains(txtSearch) || c.ProductNameEN.Contains(txtSearch)) && c.Price >= minPrice && c.Price <= maxPrice))
                        {
                            if (qCategory != null)
                            {
                                VmProductsByCategory vm = new VmProductsByCategory();
                                vm.OffProduct = item.OffProduct;
                                vm.Price = item.Price;
                                vm.ProductId = item.ProductId;
                                vm.ProductNameEN = item.ProductNameEN;
                                vm.ProductNameFA = item.ProductNameFA;
                                vm.SeeProduct = item.SeeProduct;
                                vm.FirstSubCat_FK = item.FirstSubCat_FK;
                                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                                vm.DefaultPic = "/Files/Images/Products/" + productImageName;
                                vm.TitleFirstCat = qCategory.FirstSubCatTitle;
                                ViewBag.min_Price = minPrice;
                                ViewBag.Max_Price = maxPrice;
                                ViewBag.SearchNamePro = txtSearch;
                                lstCatProducts.Add(vm);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in qProduct.Where(c => c.Price >= minPrice && c.Price <= maxPrice))
                        {
                            if (qCategory != null)
                            {
                                VmProductsByCategory vm = new VmProductsByCategory();
                                vm.OffProduct = item.OffProduct;
                                vm.Price = item.Price;
                                vm.ProductId = item.ProductId;
                                vm.ProductNameEN = item.ProductNameEN;
                                vm.ProductNameFA = item.ProductNameFA;
                                vm.SeeProduct = item.SeeProduct;
                                vm.FirstSubCat_FK = item.FirstSubCat_FK;
                                string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                                vm.DefaultPic = "/Files/Images/Products/" + productImageName;
                                vm.TitleFirstCat = qCategory.FirstSubCatTitle;
                                ViewBag.min_Price = minPrice;
                                ViewBag.Max_Price = maxPrice;
                                lstCatProducts.Add(vm);
                            }
                        }
                    }

                }
            }
            else
                return Redirect(nameof(Index));

            return View(lstCatProducts ?? null);
        }
        public IActionResult DetailsProduct(int productId = 0)
        {
            if (productId == 0)
            {
                return null;
            }

            ApplicationDbContext database = new ApplicationDbContext();

            var qProduct = database.Tbl_Products.Where(c => c.ProductId == productId && c.IsShowProduct == true)
                                                .Include(c => c.Tbl_Gallery)
                                                .Include(c => c.Tbl_Category)
                                                .Include(c => c.Tbl_FirstSubCategory)
                                                .Include(c => c.Tbl_SecondSubCategory)
                                                .Include(c => c.Tbl_LastSubCategory)
                                                .Include(c => c.Tbl_Comments)
                                                .Include(c => c.Tbl_User).SingleOrDefault();
            if (qProduct == null)
                return null;

            CategoryRepository Rep_Category = new CategoryRepository();
            CommentRepository Rep_Comment = new CommentRepository();
            GalleryRepository Rep_Gallery = new GalleryRepository();
            UserRepository Rep_User = new UserRepository();

            var qGetComment = Rep_Comment.ShowListComments(productId);
            var qGetGallery = Rep_Gallery.GetGalleryProduct(productId);
            var qGetCat = Rep_Category.ShowFirstSubCategory().Where(c => c.FirstSubCatId == qProduct.FirstSubCat_FK).FirstOrDefault();
            var qUser = Rep_User.GetUserById(qProduct.UserId_FK);//***

            //----------------------------------
            qProduct.SeeProduct = qProduct.SeeProduct + 1;
            database.Tbl_Products.Update(qProduct);
            database.SaveChanges();

            VmDetailsProduct vm = new VmDetailsProduct();
            vm.ListCommentsProduct = qGetComment == null ? null : qGetComment;
            vm.ListImagesProduct = qGetGallery == null ? null : qGetGallery;
            vm.CategoryId_FK = qProduct.CategoryId_FK;
            vm.CategoryName = qGetCat.FirstSubCatTitle;
            vm.CountProduct = qProduct.CountProduct;
            vm.DefaultPic = qProduct.DefaultPic;
            vm.Description = qProduct.Description;
            vm.FirstSubCat_FK = qProduct.FirstSubCat_FK;
            vm.LastSubCat_FK = qProduct.LastSubCat_FK;
            vm.LinkProduct = qGetCat.FirstSubCatId + "-" + qGetCat.FirstSubCatTitle;//For Example : 1-موبایل
            vm.OffProduct = qProduct.OffProduct;
            vm.Price = qProduct.Price;
            vm.ProductCode = qProduct.ProductCode;
            vm.ProductId = qProduct.ProductId;
            vm.ProductNameEN = qProduct.ProductNameEN;
            vm.ProductNameFA = qProduct.ProductNameFA;
            vm.SecondSubCat_FK = qProduct.SecondSubCat_FK;
            vm.SeeProduct = qProduct.SeeProduct;
            vm.Weight = qProduct.Weight;
            vm.SecondCatName = qProduct.Tbl_SecondSubCategory.SecondSubCatTitle;
            vm.LastCatName = qProduct.Tbl_LastSubCategory.LastSubCatTitle;
            vm.UserId_FK = qUser.Id;
            vm.Username = qUser.UserName;
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddComment(string txtTitle, string txtNote, int productId_FK, string userName)//بعدا تکمیل شود
        {
            ApplicationDbContext database = new ApplicationDbContext();
            var qVerifyProduct = database.Tbl_Products.Find(productId_FK);

            var qUser = database.Users.Where(c => c.UserName == userName).FirstOrDefault();
            database.Tbl_Comments.Add(new Comments
            {
                ConfirmComment = false,
                DateComment = DateTime.Now,
                FulllName = qUser.FirstName +" "+ qUser.LastName,
                Email=qUser.Email,
                IP = Convert.ToString(Request.HttpContext.Connection.RemotePort),
                ProductId_FK = qVerifyProduct.ProductId,
                Text = txtNote,
                Title = txtTitle,
                UserId_FK = qUser.Id,
            });
            database.SaveChanges();
            TempData["Style"] = "alert alert-success text-center";
            TempData["Message"] = "نظر شما ثبت گردید و پس از تایید نمایش داده خواهد شد";

            return RedirectToAction(nameof(DetailsProduct), "Home", new { productId = qVerifyProduct.ProductId });
        }
    }
}