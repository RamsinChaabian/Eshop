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

namespace Eshop_AspCore.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext database;

        public ProductsController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewBag.Style = ViewData["Style"];
            ViewBag.Msg = ViewData["Msg"];
            var applicationDbContext = database.Tbl_Products.Include(p => p.Tbl_Category).Include(p => p.Tbl_FirstSubCategory).Include(p => p.Tbl_LastSubCategory).Include(p => p.Tbl_SecondSubCategory).Include(p => p.Tbl_User);
            return View(await applicationDbContext.ToListAsync() ?? null);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await database.Tbl_Products
                .Include(p => p.Tbl_Category)
                .Include(p => p.Tbl_FirstSubCategory)
                .Include(p => p.Tbl_LastSubCategory)
                .Include(p => p.Tbl_SecondSubCategory)
                .Include(p => p.Tbl_User)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId_FK"] = new SelectList(database.Tbl_Category, "CatId", "CatTitle");
            //ViewData["FirstSubCat_FK"] = new SelectList(database.Tbl_FirstSubCategory, "FirstSubCatId", "FirstSubCatTitle");
            //ViewData["LastSubCat_FK"] = new SelectList(database.Tbl_LastSubCategory, "LastSubCatId", "LastSubCatTitle");
            //ViewData["SecondSubCat_FK"] = new SelectList(database.Tbl_SecondSubCategory, "SecondSubCatId", "SecondSubCatTitle");
            //ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id");
            List<Category> lstCategory = new List<Category>();
            lstCategory = (from rows in database.Tbl_Category select rows).ToList();
            lstCategory.Insert(0, new Category { CatId = 0, CatTitle = "-- انتخاب کنید --" });
            ViewBag.ListOfCategory = lstCategory;
            return View();
        }

        public JsonResult GetSubCategory2(int Cat1)
        {
            List<FirstSubCategory> lstFirstSubCategory = new List<FirstSubCategory>();
            lstFirstSubCategory = (from rows in database.Tbl_FirstSubCategory where rows.CatId_FK == Cat1 select rows).ToList();
            lstFirstSubCategory.Insert(0, new FirstSubCategory { FirstSubCatId = 0, FirstSubCatTitle = "-- انتخاب کنید --" });
            return Json(new SelectList(lstFirstSubCategory, "FirstSubCatId", "FirstSubCatTitle"));
        }
        public JsonResult GetSubCategory3(int Cat2)
        {
            List<SecondSubCategory> lstSecondSubCategory = new List<SecondSubCategory>();
            lstSecondSubCategory = (from rows in database.Tbl_SecondSubCategory where rows.FirstSubCatId_FK == Cat2 select rows).ToList();
            lstSecondSubCategory.Insert(0, new SecondSubCategory { SecondSubCatId = 0, SecondSubCatTitle = "-- انتخاب کنید --" });
            return Json(new SelectList(lstSecondSubCategory, "SecondSubCatId", "SecondSubCatTitle"));
        }

        public JsonResult GetSubCategory4(int Cat3)
        {
            List<LastSubCategory> lstLastSubCategory = new List<LastSubCategory>();
            lstLastSubCategory = (from rows in database.Tbl_LastSubCategory where rows.SecondSubCatId_FK == Cat3 select rows).ToList();
            lstLastSubCategory.Insert(0, new LastSubCategory { LastSubCatId = 0, LastSubCatTitle = "-- انتخاب کنید --" });
            return Json(new SelectList(lstLastSubCategory, "LastSubCatId", "LastSubCatTitle"));
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products)
        {

            //if (ModelState.IsValid)
            //{
            //    database.Add(products);
            //    await database.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CategoryId_FK"] = new SelectList(database.Tbl_Category, "CatId", "CatTitle", products.CategoryId_FK);
            //ViewData["FirstSubCat_FK"] = new SelectList(database.Tbl_FirstSubCategory, "FirstSubCatId", "FirstSubCatTitle", products.FirstSubCat_FK);
            //ViewData["LastSubCat_FK"] = new SelectList(database.Tbl_LastSubCategory, "LastSubCatId", "LastSubCatTitle", products.LastSubCat_FK);
            //ViewData["SecondSubCat_FK"] = new SelectList(database.Tbl_SecondSubCategory, "SecondSubCatId", "SecondSubCatTitle", products.SecondSubCat_FK);
            //ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", products.UserId_FK);

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(AccountController.Login), "Account");
            string currentUser = User.Identity.Name;
            var userId = database.Users.Where(u => u.UserName == currentUser).SingleOrDefault().Id;
            //-------------Set validation Category-------------//

            if (products.CategoryId_FK == 0)
                ModelState.AddModelError("", "لطفا دسته اصلی را انتخاب نمایید");
            else if (products.FirstSubCat_FK == 0)
                ModelState.AddModelError("", "لطفا زیر دسته اول را انتخاب نمایید");
            else if (products.SecondSubCat_FK == 0)
                ModelState.AddModelError("", "لطفا زیر دسته دوم را انتخاب نمایید");
            else if (products.LastSubCat_FK == 0)
                ModelState.AddModelError("", "لطفا زیر دسته آخر را انتخاب نمایید");

            //------------------------------------------------//

            var category = HttpContext.Request.Form["Cat1"].ToString();
            var firstSubCategory = HttpContext.Request.Form["Cat2"].ToString();
            var secoundSubCategory = HttpContext.Request.Form["Cat3"].ToString();
            var lastSubCategory = HttpContext.Request.Form["Cat4"].ToString();

            Products tbl_products = new Products()
            {
                CategoryId_FK = Convert.ToInt32(category),
                CountProduct = products.CountProduct,
                DateProduct = DateTime.Now,
                Description = products.Description,
                DefaultPic = "",
                IsShowProduct = products.IsShowProduct,
                FirstSubCat_FK = Convert.ToInt32(firstSubCategory),
                SecondSubCat_FK = Convert.ToInt32(secoundSubCategory),
                LastSubCat_FK = Convert.ToInt32(lastSubCategory),
                OffProduct = products.OffProduct,
                Price = products.Price,
                ProductCode = "DYP" + ((DateTime.Now.Second * 2) + (DateTime.Now.Minute + 100)).ToString(),
                ProductNameEN = products.ProductNameEN,
                ProductNameFA = products.ProductNameFA,
                SeeProduct = 0,
                UserId_FK = userId,
                Weight = products.Weight,

            };
            database.Tbl_Products.Add(tbl_products);
            await database.SaveChangesAsync();
            ViewData["Style"] = "alert alert-success";
            ViewData["Msg"] = "محصول با موفقیت ثبت گردید";
            var productId = tbl_products.ProductId;
            return RedirectToAction("Create", "Galleries", new { ProductID = productId });
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await database.Tbl_Products.SingleOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryId_FK"] = new SelectList(database.Tbl_Category, "CatId", "CatTitle", products.CategoryId_FK);
            ViewData["FirstSubCat_FK"] = new SelectList(database.Tbl_FirstSubCategory, "FirstSubCatId", "FirstSubCatTitle", products.FirstSubCat_FK);
            ViewData["LastSubCat_FK"] = new SelectList(database.Tbl_LastSubCategory, "LastSubCatId", "LastSubCatTitle", products.LastSubCat_FK);
            ViewData["SecondSubCat_FK"] = new SelectList(database.Tbl_SecondSubCategory, "SecondSubCatId", "SecondSubCatTitle", products.SecondSubCat_FK);
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", products.UserId_FK);

            List<Category> lstCategory = new List<Category>();
            lstCategory = (from rows in database.Tbl_Category select rows).ToList();
            lstCategory.Insert(0, new Category { CatId = 0, CatTitle = "-- انتخاب کنید --" });
            ViewBag.ListOfCategory = lstCategory;

            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!User.Identity.IsAuthenticated)
                        return RedirectToAction(nameof(AccountController.Login), "Account");
                    string currentUser = User.Identity.Name;
                    var userId = database.Users.Where(u => u.UserName == currentUser).SingleOrDefault().Id;
                    //-------------Set validation Category-------------//
                    if (products.CategoryId_FK == 0)
                        ModelState.AddModelError("", "لطفا دسته اصلی را انتخاب نمایید");
                    else if (products.FirstSubCat_FK == 0)
                        ModelState.AddModelError("", "لطفا زیر دسته اول را انتخاب نمایید");
                    else if (products.SecondSubCat_FK == 0)
                        ModelState.AddModelError("", "لطفا زیر دسته دوم را انتخاب نمایید");
                    else if (products.LastSubCat_FK == 0)
                        ModelState.AddModelError("", "لطفا زیر دسته آخر را انتخاب نمایید");

                    //------------------------------------------------//

                    var category = HttpContext.Request.Form["Cat1"].ToString();
                    var firstSubCategory = HttpContext.Request.Form["Cat2"].ToString();
                    var secoundSubCategory = HttpContext.Request.Form["Cat3"].ToString();
                    var lastSubCategory = HttpContext.Request.Form["Cat4"].ToString();

                    var qProduct = database.Tbl_Products.Where(c => c.ProductId == id).FirstOrDefault();
                    if (qProduct == null)
                        return NotFound();

                    //--------------------Update Product--------------------//
                    qProduct.CategoryId_FK = Convert.ToInt32(category);
                    qProduct.CountProduct = products.CountProduct;
                    qProduct.Description = products.Description;
                    qProduct.IsShowProduct = products.IsShowProduct;
                    qProduct.FirstSubCat_FK = Convert.ToInt32(firstSubCategory);
                    qProduct.SecondSubCat_FK = Convert.ToInt32(secoundSubCategory);
                    qProduct.LastSubCat_FK = Convert.ToInt32(lastSubCategory);
                    qProduct.OffProduct = products.OffProduct;
                    qProduct.Price = products.Price;
                    qProduct.ProductNameEN = products.ProductNameEN;
                    qProduct.ProductNameFA = products.ProductNameFA;
                    qProduct.UserId_FK = userId;
                    qProduct.Weight = products.Weight;

                    database.Update(qProduct);
                    await database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["Style"] = "alert alert-success";
                ViewData["Msg"] = "محصول با موفقیت ویرایش گردید";
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId_FK"] = new SelectList(database.Tbl_Category, "CatId", "CatTitle", products.CategoryId_FK);
            //ViewData["FirstSubCat_FK"] = new SelectList(database.Tbl_FirstSubCategory, "FirstSubCatId", "FirstSubCatTitle", products.FirstSubCat_FK);
            //ViewData["LastSubCat_FK"] = new SelectList(database.Tbl_LastSubCategory, "LastSubCatId", "LastSubCatTitle", products.LastSubCat_FK);
            //ViewData["SecondSubCat_FK"] = new SelectList(database.Tbl_SecondSubCategory, "SecondSubCatId", "SecondSubCatTitle", products.SecondSubCat_FK);
            //ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", products.UserId_FK);

            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await database.Tbl_Products
                .Include(p => p.Tbl_Category)
                .Include(p => p.Tbl_FirstSubCategory)
                .Include(p => p.Tbl_LastSubCategory)
                .Include(p => p.Tbl_SecondSubCategory)
                .Include(p => p.Tbl_User)
                .SingleOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await database.Tbl_Products.SingleOrDefaultAsync(m => m.ProductId == id);
            database.Tbl_Products.Remove(products);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return database.Tbl_Products.Any(e => e.ProductId == id);
        }
    }
}
