using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using Eshop_AspCore.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace EShop_AspCore.Controllers
{
    [Authorize]
    public class LastSubCategoriesController : Controller
    {
        private readonly ApplicationDbContext database;

        public LastSubCategoriesController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: LastSubCategories
        public async Task<IActionResult> Index(int? secondCatId)
        {
            var currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser).SingleOrDefault();
            var qRole = database.Tbl_Role.Where(c => c.RoleName == "Admin").FirstOrDefault();
            var qAccess = database.Tbl_UserAccess.Where(c => c.UserId_FK == qUser.Id && c.RoleId_FK == qRole.RoleId).SingleOrDefault();
            if (qAccess != null)
            {
                if (secondCatId == null)
                {
                    return View(await database.Tbl_LastSubCategory.ToListAsync());
                }
                else
                {
                    return View(await database.Tbl_LastSubCategory.Where(c => c.SecondSubCatId_FK == secondCatId).ToListAsync());
                }
            }
            else
            {
                TempData["ErrorAccess"] = "شما سطح دسترسی به این بخش را ندارین";
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: LastSubCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LastSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastSubCatId,SecondSubCatId_FK,LastSubCatTitle")] LastSubCategory lastSubCategory)
        {
            if (ModelState.IsValid)
            {
                database.Add(lastSubCategory);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lastSubCategory);
        }

        // GET: LastSubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lastSubCategory = await database.Tbl_LastSubCategory.SingleOrDefaultAsync(m => m.LastSubCatId == id);
            if (lastSubCategory == null)
            {
                return NotFound();
            }
            return View(lastSubCategory);
        }

        // POST: LastSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LastSubCatId,SecondSubCatId_FK,LastSubCatTitle")] LastSubCategory lastSubCategory)
        {
            if (id != lastSubCategory.LastSubCatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(lastSubCategory);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LastSubCategoryExists(lastSubCategory.LastSubCatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lastSubCategory);
        }

        // GET: LastSubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lastSubCategory = await database.Tbl_LastSubCategory
                .SingleOrDefaultAsync(m => m.LastSubCatId == id);
            if (lastSubCategory == null)
            {
                return NotFound();
            }

            return View(lastSubCategory);
        }

        // POST: LastSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lastSubCategory = await database.Tbl_LastSubCategory.SingleOrDefaultAsync(m => m.LastSubCatId == id);
            database.Tbl_LastSubCategory.Remove(lastSubCategory);
            await database.SaveChangesAsync();
            TempData["Style"] = "alert alert-success";
            TempData["Msg"] = "دسته با موفقیت حذف گردید";
            return RedirectToAction(nameof(Index));
        }

        private bool LastSubCategoryExists(int id)
        {
            return database.Tbl_LastSubCategory.Any(e => e.LastSubCatId == id);
        }
    }
}
