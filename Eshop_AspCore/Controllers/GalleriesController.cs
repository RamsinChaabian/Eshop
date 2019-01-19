using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Eshop_AspCore.Controllers
{
    [Authorize]
    public class GalleriesController : Controller
    {
        private readonly ApplicationDbContext database;
        private IHostingEnvironment environment;
        public GalleriesController(ApplicationDbContext context, IHostingEnvironment _environment)
        {
            database = context;
            environment = _environment;
        }

        // GET: Galleries
        public async Task<IActionResult> Index(int page = 1)
        {
            var qListGallery = await database.Tbl_Gallery.Where(c => c.DefaultPicProduct == true).Include(g => g.Tbl_Products).ToListAsync();
            int Take = 3;
            int Skip = (Take * page) - Take;

            ViewBag.CountProduct = qListGallery.Count();

            ViewBag.Take = Take;
            ViewBag.Skip = Skip;
            ViewBag.page = page;

            return View(qListGallery.OrderByDescending(c => c.PictureId).Skip(Skip).Take(Take));
        }

        // GET: Galleries/Create
        public IActionResult Create(int ProductID)
        {
            //ViewData["ProductId_FK"] = new SelectList(database.Tbl_Products, "ProductId", "Description");

            var q = database.Tbl_Products.Where(c => c.ProductId == ProductID).SingleOrDefault();
            if (q == null)
                return RedirectToAction(nameof(GalleriesController.Index), "Galleries");

            ViewBag.ProductId = q.ProductId;
            ViewBag.ProductName = q.ProductNameFA;
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile[] GalleryProduct, int productID)
        {
            if (GalleryProduct[0] == null)
            {
                TempData["Style"] = "alert alert-danger";
                TempData["Msg"] = "لطفا یک تصویر را انتخاب کنید";
                return View();
            }
            else
            {
                var qProduct = database.Tbl_Products.Where(c => c.ProductId == productID).FirstOrDefault();
                Gallery tbl_gallery = new Gallery();
                string fileNamePic = Guid.NewGuid().ToString().Replace("-", "") + GalleryProduct[0].FileName.ToLower();
                var uploadsPicMain = Path.Combine(environment.WebRootPath, "Files\\Images\\Products");
                using (var fileStreamMain = new FileStream(Path.Combine(uploadsPicMain, fileNamePic), FileMode.Create))
                {
                    await GalleryProduct[0].CopyToAsync(fileStreamMain);
                }
                tbl_gallery.DefaultPicProduct = true;
                tbl_gallery.ProductId_FK = qProduct.ProductId;
                tbl_gallery.PictureName = fileNamePic;
                tbl_gallery.TitlePicture = qProduct.ProductNameFA;
                database.Tbl_Gallery.Add(tbl_gallery);
                               
                await database.SaveChangesAsync();

                List<Gallery> lstGallery = new List<Gallery>();
                foreach (var item in GalleryProduct)
                {
                    Gallery g = new Gallery();

                    if (!Directory.Exists(@"" + environment.WebRootPath + "/Files/Images/Products/Gallery"))
                    {
                        Directory.CreateDirectory(@"" + environment.WebRootPath + "/Files/Images/Products/Gallery");
                    }

                    string fileName = Guid.NewGuid().ToString().Replace("-", "") + GalleryProduct[0].FileName.ToLower();
                    var uploadPic = Path.Combine(environment.WebRootPath, "Files\\Images\\Products\\Gallery");
                    using (var fileStream = new FileStream(Path.Combine(uploadPic, fileName), FileMode.Create))
                    {
                        await item.CopyToAsync(fileStream);
                    }
                    g.ProductId_FK = qProduct.ProductId;
                    g.PictureName = fileName;
                    g.TitlePicture = qProduct.ProductNameFA;
                    g.DefaultPicProduct = false;
                    lstGallery.Add(g);
                }

                database.Tbl_Gallery.AddRange(lstGallery.AsEnumerable());

                qProduct.DefaultPic = fileNamePic;

                await database.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

        }

        // GET: Galleries/Edit/5
        public async Task<IActionResult> Edit(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            var gallery = await database.Tbl_Gallery.Where(m => m.ProductId_FK == productId).ToListAsync();
            if (gallery == null)
            {
                return NotFound();
            }

            List<Gallery> lstGallery = new List<Gallery>();
            foreach (var item in gallery)
            {
                Gallery g = new Gallery();
                g.TitlePicture = item.TitlePicture;
                g.PictureName = item.PictureName;
                g.DefaultPicProduct = item.DefaultPicProduct;
                g.ProductId_FK = item.ProductId_FK;
                g.PictureId = item.PictureId;
                lstGallery.Add(g);

            }

            return View(lstGallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile[] GalleryProdouct, int productId)
        {
            if (GalleryProdouct[0] == null)
            {
                return Redirect(nameof(Index));
            }
            else
            {
                var qProduct = database.Tbl_Products.Where(a => a.ProductId == productId).FirstOrDefault();
                List<Gallery> lstgallery = new List<Gallery>();

                foreach (var item in GalleryProdouct)
                {
                    string FileNamepic = Guid.NewGuid().ToString().Replace("-", "") + item.FileName.ToLower();
                    var uploadspic = Path.Combine(environment.WebRootPath, "Files\\Images\\Products\\Gallery");
                    using (var fileStream = new FileStream(Path.Combine(uploadspic, FileNamepic), FileMode.Create))
                    {
                        await item.CopyToAsync(fileStream);
                    }

                    Gallery g = new Gallery();
                    g.DefaultPicProduct = false;
                    g.ProductId_FK = qProduct.ProductId;
                    g.PictureName = FileNamepic;
                    g.TitlePicture = qProduct.ProductNameFA;

                    lstgallery.Add(g);

                }

                database.Tbl_Gallery.AddRange(lstgallery.AsEnumerable());
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] galleryId, string[] ChekboxDelete, int productId)
        {
            if (galleryId.Count() == 0 || ChekboxDelete.Count() == 0)
                return RedirectToAction(nameof(Edit), "Galleries", new { id = productId });


            foreach (var item in galleryId.ToList())
            {
                foreach (var item1 in ChekboxDelete)
                {
                    if (item == item1)
                    {
                        var qdelete = await database.Tbl_Gallery.Where(a => a.DefaultPicProduct == false).SingleOrDefaultAsync(c => c.PictureId == Convert.ToInt32(item1));
                        var DeleteImage = Path.Combine(environment.WebRootPath, "Files\\Images\\Products\\Gallery\\" + qdelete.PictureName);

                        if (System.IO.File.Exists(DeleteImage))
                        {
                            System.IO.File.Delete(DeleteImage);

                        }

                        if (qdelete != null)
                            database.Tbl_Gallery.Remove(qdelete);

                        await database.SaveChangesAsync();

                    }
                    //else
                    //{
                    //    return RedirectToAction(nameof(Edit), "Galleries", new { id = IdPro });
                    //}
                }

            }
            return RedirectToAction(nameof(Edit), "Galleries", new { productId = productId });

        }

        //// POST: Galleries/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var gallery = await database.Tbl_Gallery.SingleOrDefaultAsync(m => m.PictureId == id);
        //    database.Tbl_Gallery.Remove(gallery);
        //    await database.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool GalleryExists(int id)
        {
            return database.Tbl_Gallery.Any(e => e.PictureId == id);
        }
    }
}
