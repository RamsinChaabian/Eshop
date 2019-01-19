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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext database;

        public CommentsController(ApplicationDbContext context)
        {
            database = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = database.Tbl_Comments.Include(c => c.Tbl_Products).Include(c => c.Tbl_User);
            return View(await applicationDbContext.OrderByDescending(c => c.CommentId).ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await database.Tbl_Comments
                .Include(c => c.Tbl_Products)
                .Include(c => c.Tbl_User)
                .SingleOrDefaultAsync(m => m.CommentId == id);
            if (comments == null)
            {
                return NotFound();
            }

            return View(comments);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["ProductId_FK"] = new SelectList(database.Tbl_Products, "ProductId", "Description");
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,ProductId_FK,UserId_FK,FulllName,Email,Title,Text,IP,DateComment,ConfirmComment")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                database.Add(comments);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId_FK"] = new SelectList(database.Tbl_Products, "ProductId", "Description", comments.ProductId_FK);
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", comments.UserId_FK);
            return View(comments);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await database.Tbl_Comments.SingleOrDefaultAsync(m => m.CommentId == id);
            if (comments == null)
            {
                return NotFound();
            }
            ViewData["ProductId_FK"] = new SelectList(database.Tbl_Products, "ProductId", "Description", comments.ProductId_FK);
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", comments.UserId_FK);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,ProductId_FK,UserId_FK,FulllName,Email,Title,Text,IP,DateComment,ConfirmComment")] Comments comments)
        {
            if (id != comments.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    database.Update(comments);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentsExists(comments.CommentId))
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
            ViewData["ProductId_FK"] = new SelectList(database.Tbl_Products, "ProductId", "Description", comments.ProductId_FK);
            ViewData["UserId_FK"] = new SelectList(database.Users, "Id", "Id", comments.UserId_FK);
            return View(comments);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var comments = await database.Tbl_Comments.SingleOrDefaultAsync(m => m.CommentId == id);
            database.Tbl_Comments.Remove(comments);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentsExists(int id)
        {
            return database.Tbl_Comments.Any(e => e.CommentId == id);
        }
        public async Task<IActionResult> ConfirmComment(int commentId)
        {
            var qComment = database.Tbl_Comments.Where(c => c.CommentId == commentId).SingleOrDefault();
            if (qComment.ConfirmComment == false)
            {
                qComment.ConfirmComment = true;
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                qComment.ConfirmComment = false;
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
