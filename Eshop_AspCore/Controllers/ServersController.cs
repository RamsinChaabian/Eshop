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
    public class ServersController : Controller
    {
        private readonly ApplicationDbContext database;

        public ServersController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: Servers
        public async Task<IActionResult> Index()
        {
            return View(await database.Tbl_Server.ToListAsync());
        }

        // GET: Servers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var server = await database.Tbl_Server.SingleOrDefaultAsync(m => m.ServerId == id);
            if (server == null)
            {
                return NotFound();
            }
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServerId,ServerTitle,IP,FtpUsername,FtpPassword,Path,Type,HttpDomain")] Server server)
        {
            if (id != server.ServerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(server);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServerExists(server.ServerId))
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
            return View(server);
        }
        private bool ServerExists(int id)
        {
            return database.Tbl_Server.Any(e => e.ServerId == id);
        }
    }
}
