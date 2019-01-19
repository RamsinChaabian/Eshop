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
    public class SecondSubCategoriesController : Controller
    {
        private readonly ApplicationDbContext database;

        public SecondSubCategoriesController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: SecondSubCategories
        public async Task<IActionResult> Index(int? firstCatId)
        {
            var currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser).SingleOrDefault();
            var qRole = database.Tbl_Role.Where(c => c.RoleName == "Admin").FirstOrDefault();
            var qAccess = database.Tbl_UserAccess.Where(c => c.UserId_FK == qUser.Id && c.RoleId_FK == qRole.RoleId).SingleOrDefault();
            if (qAccess != null)
            {
                if (firstCatId == null)
                {
                    return View(await database.Tbl_SecondSubCategory.ToListAsync());
                }
                else
                {
                    return View(await database.Tbl_SecondSubCategory.Where(c => c.FirstSubCatId_FK == firstCatId).ToListAsync());

                }
            }
            else
            {
                TempData["ErrorAccess"] = "شما سطح دسترسی به این بخش را ندارین";
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: SecondSubCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecondSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SecondSubCatId,FirstSubCatId_FK,SecondSubCatTitle")] SecondSubCategory secondSubCategory)
        {
            if (ModelState.IsValid)
            {
                database.Add(secondSubCategory);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(secondSubCategory);
        }

        // GET: SecondSubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secondSubCategory = await database.Tbl_SecondSubCategory.SingleOrDefaultAsync(m => m.SecondSubCatId == id);
            if (secondSubCategory == null)
            {
                return NotFound();
            }
            return View(secondSubCategory);
        }

        // POST: SecondSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SecondSubCatId,FirstSubCatId_FK,SecondSubCatTitle")] SecondSubCategory secondSubCategory)
        {
            if (id != secondSubCategory.SecondSubCatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(secondSubCategory);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecondSubCategoryExists(secondSubCategory.SecondSubCatId))
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
            return View(secondSubCategory);
        }

        // GET: SecondSubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secondSubCategory = await database.Tbl_SecondSubCategory
                .SingleOrDefaultAsync(m => m.SecondSubCatId == id);
            if (secondSubCategory == null)
            {
                return NotFound();
            }

            return View(secondSubCategory);
        }

        // POST: SecondSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var secondSubCategory = await database.Tbl_SecondSubCategory.SingleOrDefaultAsync(m => m.SecondSubCatId == id);
            database.Tbl_SecondSubCategory.Remove(secondSubCategory);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecondSubCategoryExists(int id)
        {
            return database.Tbl_SecondSubCategory.Any(e => e.SecondSubCatId == id);
        }
    }
}
