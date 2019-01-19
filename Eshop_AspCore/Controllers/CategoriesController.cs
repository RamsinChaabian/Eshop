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
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext database;

        public CategoriesController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser).SingleOrDefault();
            var qRole = database.Tbl_Role.Where(c => c.RoleName == "Admin").FirstOrDefault();
            var qAccess = database.Tbl_UserAccess.Where(c => c.UserId_FK == qUser.Id && c.RoleId_FK == qRole.RoleId).SingleOrDefault();
            if (qAccess != null)
            {
                return View(await database.Tbl_Category.ToListAsync());
            }
            else
            {
                TempData["ErrorAccess"] = "شما سطح دسترسی به این بخش را ندارین";
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await database.Tbl_Category
                .SingleOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatTitle")] Category category)
        {
            if (ModelState.IsValid)
            {
                database.Add(category);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await database.Tbl_Category.SingleOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,CatTitle")] Category category)
        {
            if (id != category.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(category);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CatId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await database.Tbl_Category
                .SingleOrDefaultAsync(m => m.CatId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await database.Tbl_Category.SingleOrDefaultAsync(m => m.CatId == id);
            database.Tbl_Category.Remove(category);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return database.Tbl_Category.Any(e => e.CatId == id);
        }
    }
}
