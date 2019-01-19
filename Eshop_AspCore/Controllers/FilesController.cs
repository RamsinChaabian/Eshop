using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;

namespace Eshop_AspCore.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext database;

        public FilesController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: Files
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = database.Tbl_Files.Include(f => f.Tbl_Server);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await database.Tbl_Files
                .Include(f => f.Tbl_Server)
                .SingleOrDefaultAsync(m => m.FileId == id);
            if (files == null)
            {
                return NotFound();
            }

            return View(files);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            ViewBag.ServerId_FK = new SelectList(database.Tbl_Server, "ServerId", "FtpPassword");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileId,ServerId_FK,Title,FileName,Alt")] Files files)
        {
            if (ModelState.IsValid)
            {
                database.Add(files);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ServerId_FK = new SelectList(database.Tbl_Server, "ServerId", "FtpPassword", files.ServerId_FK);
            return View(files);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await database.Tbl_Files.SingleOrDefaultAsync(m => m.FileId == id);
            if (files == null)
            {
                return NotFound();
            }
            ViewBag.ServerId_FK = new SelectList(database.Tbl_Server, "ServerId", "FtpPassword", files.ServerId_FK);
            return View(files);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileId,ServerId_FK,Title,FileName,Alt")] Files files)
        {
            if (id != files.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(files);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilesExists(files.FileId))
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
           ViewBag.ServerId_FK = new SelectList(database.Tbl_Server, "ServerId", "FtpPassword", files.ServerId_FK);
            return View(files);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var files = await database.Tbl_Files
                .Include(f => f.Tbl_Server)
                .SingleOrDefaultAsync(m => m.FileId == id);
            if (files == null)
            {
                return NotFound();
            }

            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var files = await database.Tbl_Files.SingleOrDefaultAsync(m => m.FileId == id);
            database.Tbl_Files.Remove(files);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilesExists(int id)
        {
            return database.Tbl_Files.Any(e => e.FileId == id);
        }
    }
}
