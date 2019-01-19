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
    public class UserAccessesController : Controller
    {
        private readonly ApplicationDbContext database;

        public UserAccessesController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: UserAccesses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = database.Tbl_UserAccess.Include(u => u.Tbl_Role).Include(u => u.Tbl_User);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: UserAccesses/Create
        public IActionResult Create()
        {
            ViewData["RoleId_FK"] = new SelectList(database.Tbl_Role, "RoleId", "RoleTitle");
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id");
            return View();
        }

        // POST: UserAccesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserId_FK,RoleId_FK,AddPost,DeletePost,EditPost")] UserAccess userAccess)
        {
            if (ModelState.IsValid)
            {
                database.Add(userAccess);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId_FK"] = new SelectList(database.Tbl_Role, "RoleId", "RoleTitle", userAccess.RoleId_FK);
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", userAccess.UserId_FK);
            return View(userAccess);
        }

        // GET: UserAccesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAccess = await database.Tbl_UserAccess.SingleOrDefaultAsync(m => m.ID == id);
            if (userAccess == null)
            {
                return NotFound();
            }
            ViewData["RoleId_FK"] = new SelectList(database.Tbl_Role, "RoleId", "RoleTitle", userAccess.RoleId_FK);
            ViewData["UserId_FK"] = userAccess.UserId_FK;
            return View(userAccess);
        }

        // POST: UserAccesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserId_FK,RoleId_FK,AddPost,DeletePost,EditPost")] UserAccess userAccess)
        {
            if (id != userAccess.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(userAccess);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccessExists(userAccess.ID))
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
            ViewData["RoleId_FK"] = new SelectList(database.Tbl_Role, "RoleId", "RoleName", userAccess.RoleId_FK);
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", userAccess.UserId_FK);
            return View(userAccess);
        }

        private bool UserAccessExists(int id)
        {
            return database.Tbl_UserAccess.Any(e => e.ID == id);
        }
    }
}
