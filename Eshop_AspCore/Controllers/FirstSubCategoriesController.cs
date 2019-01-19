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
    public class FirstSubCategoriesController : Controller
    {
        private readonly ApplicationDbContext database;
        private IHostingEnvironment environment;
        public FirstSubCategoriesController(ApplicationDbContext context, IHostingEnvironment _environment)
        {
            database = context;
            environment = _environment;
        }

        // GET: FirstSubCategories
        public async Task<IActionResult> Index(int? catId)
        {
            var currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser).SingleOrDefault();
            var qRole = database.Tbl_Role.Where(c => c.RoleName == "Admin").FirstOrDefault();
            var qAccess = database.Tbl_UserAccess.Where(c => c.UserId_FK == qUser.Id && c.RoleId_FK == qRole.RoleId).SingleOrDefault();
            if (qAccess != null)
            {
                if (catId == null)
                {
                    return View(await database.Tbl_FirstSubCategory.ToListAsync());
                }
                else
                {
                    return View(await database.Tbl_FirstSubCategory.Where(c=>c.CatId_FK==catId).ToListAsync());
                }
            }
            else
            {
                TempData["ErrorAccess"] = "شما سطح دسترسی به این بخش را ندارین";
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }
        // GET: FirstSubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firstSubCategory = await database.Tbl_FirstSubCategory
                .SingleOrDefaultAsync(m => m.FirstSubCatId == id);
            if (firstSubCategory == null)
            {
                return NotFound();
            }

            return View(firstSubCategory);
        }

        // GET: FirstSubCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FirstSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstSubCatId,CatId_FK,FirstSubCatTitle,Picture")] FirstSubCategory firstSubCategory, IFormFile CatPic)
        {
            if (ModelState.IsValid)
            {
                string fileName = "";
                string uploadPath = "";

                if (CatPic != null && CatPic.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString().Replace("-", "") + CatPic.FileName.ToLower();
                    uploadPath = Path.Combine(environment.WebRootPath, "Files\\Images\\");
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await CatPic.CopyToAsync(fileStream);
                    }
                    database.Tbl_FirstSubCategory.Add(new FirstSubCategory()
                    {
                        CatId_FK = firstSubCategory.CatId_FK,
                        FirstSubCatTitle = firstSubCategory.FirstSubCatTitle,
                        Picture = fileName,
                    });
                }
                //database.Add(firstSubCategory);
                await database.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            //return View(firstSubCategory);
            return RedirectToAction(nameof(Index));
        }

        // GET: FirstSubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firstSubCategory = await database.Tbl_FirstSubCategory.SingleOrDefaultAsync(m => m.FirstSubCatId == id);
            if (firstSubCategory == null)
            {
                return NotFound();
            }
            return View(firstSubCategory);
        }

        // POST: FirstSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstSubCatId,CatId_FK,FirstSubCatTitle,Picture")] FirstSubCategory firstSubCategory, IFormFile CatPic)
        {
            var q = database.Tbl_FirstSubCategory.Where(c => c.FirstSubCatId == id).SingleOrDefault();

            if (id != q.FirstSubCatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string oldFilename = q.Picture;
                    string fileName = "";
                    string uploadPath = "";
                    database.Update(q);

                    if (CatPic != null && CatPic.Length > 0)
                    {
                        string RemovePic = Path.Combine(environment.WebRootPath, "Files\\Images\\" + oldFilename);
                        if (System.IO.File.Exists(RemovePic))
                        {
                            System.IO.File.Delete(RemovePic);
                        }
                        fileName = Guid.NewGuid().ToString().Replace("-", "") + CatPic.FileName.ToLower();
                        uploadPath = Path.Combine(environment.WebRootPath, "Files\\Images\\");
                        using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                        {
                            await CatPic.CopyToAsync(fileStream);
                        }

                    }
                    q.CatId_FK = firstSubCategory.CatId_FK;
                    q.FirstSubCatTitle = firstSubCategory.FirstSubCatTitle;
                    q.Picture = fileName == "" ? oldFilename : fileName;
                    database.Update(q);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirstSubCategoryExists(q.FirstSubCatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
            //return View(firstSubCategory);
        }

        // GET: FirstSubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firstSubCategory = await database.Tbl_FirstSubCategory
                .SingleOrDefaultAsync(m => m.FirstSubCatId == id);
            if (firstSubCategory == null)
            {
                return NotFound();
            }

            return View(firstSubCategory);
        }

        // POST: FirstSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var q = database.Tbl_SecondSubCategory.Where(c => c.FirstSubCatId_FK == id).ToList();
            if (q != null && q.Count > 1)
            {
                TempData["Style"] = "alert alert-danger";
                TempData["Msg"] = "برای حذف دسته ابتدا باید تمامی زیردسته های آن را حذف نمایید";
            }
            else
            {
                var firstSubCategory = await database.Tbl_FirstSubCategory.SingleOrDefaultAsync(m => m.FirstSubCatId == id);
                database.Tbl_FirstSubCategory.Remove(firstSubCategory);
                await database.SaveChangesAsync();
                TempData["Style"] = "alert alert-success";
                TempData["Msg"] = "دسته با موفقیت حذف گردید";
            }
            return RedirectToAction(nameof(Index));
        }
        private bool FirstSubCategoryExists(int id)
        {
            return database.Tbl_FirstSubCategory.Any(e => e.FirstSubCatId == id);
        }
    }
}
