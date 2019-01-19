using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Eshop_AspCore.Classes;
using System.IO;

namespace Eshop_AspCore.Controllers
{
    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext database;
        private IHostingEnvironment hostingEnvironment;
        public SlidersController(ApplicationDbContext context, IHostingEnvironment _hostingEnvironment)
        {
            database = context;
            hostingEnvironment = _hostingEnvironment;
        }

        // GET: Sliders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = database.Tbl_Slider.Include(s => s.Tbl_Files).ThenInclude(c => c.Tbl_Server);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sliders/Create
        public IActionResult Create()
        {
            ViewData["FileId_FK"] = new SelectList(database.Tbl_Files, "FileId", "FileName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider, IFormFile ImageUpload)
        {
            if (ModelState.IsValid)
            {
                Slider tbl_Slider = new Slider();
                tbl_Slider.Description = slider.Description;
                tbl_Slider.Link = slider.Link;
                tbl_Slider.SliderTitle = slider.SliderTitle;
                tbl_Slider.Sort = slider.Sort;

                if (slider.ServerUpload == true)
                {
                    if (ImageUpload == null && ImageUpload.Length < 0)
                        return Redirect(nameof(Create));
                    Files files = new Files();
                    FtpWorker ftp = new FtpWorker();
                    string fileName = Guid.NewGuid().ToString().Replace("-", "") + ImageUpload.FileName.ToLower();
                    int FtpID = ftp.Upload("Content", fileName, ImageUpload.OpenReadStream());
                    if (FtpID != -1)
                    {
                        files.ServerId_FK = FtpID;
                        files.FileName = fileName;
                        files.Title = slider.SliderTitle;
                        files.Alt = slider.SliderTitle;
                        database.Tbl_Files.Add(files);
                        await database.SaveChangesAsync();
                    }
                }
                else
                {
                    string uploads = Path.Combine(hostingEnvironment.WebRootPath, "Files\\Images\\Slider");
                    if (ImageUpload == null && ImageUpload.Length == 0)
                        return RedirectToAction(nameof(Create));

                    string fileName = Guid.NewGuid().ToString().Replace("-", "") + ImageUpload.FileName.ToLower();
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await ImageUpload.CopyToAsync(fileStream);
                    }

                    int GetServerId = database.Tbl_Server.Where(c => c.Type == "Content").FirstOrDefault().ServerId;
                    Files files = new Files();
                    files.ServerId_FK = GetServerId;
                    files.FileName = fileName;
                    files.Title = slider.SliderTitle;
                    files.Alt = slider.SliderTitle;
                    database.Tbl_Files.Add(files);
                    await database.SaveChangesAsync();

                }

                tbl_Slider.FileId_FK = database.Tbl_Files.OrderByDescending(c => c.FileId).FirstOrDefault().FileId;
                tbl_Slider.ServerUpload = slider.ServerUpload;
                database.Add(tbl_Slider);
                await database.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FileId_FK"] = new SelectList(database.Tbl_Files, "FileId", "FileName", slider.FileId_FK);
            return View(slider);
        }

        // GET: Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await database.Tbl_Slider.Include(c => c.Tbl_Files).ThenInclude(c => c.Tbl_Server).SingleOrDefaultAsync(m => m.SliderId == id);
            if (slider == null)
            {
                return NotFound();
            }
            ViewData["FileId_FK"] = new SelectList(database.Tbl_Files, "FileId", "FileName", slider.FileId_FK);
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Slider slider, IFormFile ImageUpload)
        {
            if (id != slider.SliderId)
            {
                return NotFound();
            }
            var tbl_slider = database.Tbl_Slider.Where(c => c.SliderId == id).Include(c => c.Tbl_Files).ThenInclude(c => c.Tbl_Server).SingleOrDefault();
            if (slider == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_slider.Link = slider.Link;
                    tbl_slider.SliderTitle = slider.SliderTitle;
                    tbl_slider.Description = slider.Description;
                    tbl_slider.Sort = slider.Sort;

                    if (ImageUpload != null && !string.IsNullOrEmpty(ImageUpload.FileName) && ImageUpload.Length > 0)
                    {
                        if (slider.ServerUpload == true)
                        {
                            var img = database.Tbl_Files.Where(c => c.FileId == tbl_slider.FileId_FK).Include(c => c.Tbl_Server).SingleOrDefault();
                            if (img == null)
                            {
                                return NotFound();
                            }

                            FtpWorker ftp = new FtpWorker();

                            //-----------------Delete Image From Server && Local--------------------//
                            //if (tbl_slider.ServerUpload == true)
                            //{
                            //    int FtpId = database.Tbl_Server.Where(c => c.Type == "Content").SingleOrDefault().ServerId;
                            //    var result = ftp.Remove(FtpId, tbl_slider.Tbl_Files.FileName);
                            //    if (result == false)
                            //        return RedirectToAction(nameof(Edit), new { id = tbl_slider.SliderId });
                            //}
                            //else
                            //{
                            //    var getFileName = Path.Combine(hostingEnvironment.WebRootPath, "Files\\Images\\Slider\\" + tbl_slider.Tbl_Files.FileName);
                            //    if (System.IO.File.Exists(getFileName))
                            //    {
                            //        System.IO.File.Delete(getFileName);
                            //    }

                            //}
                            //--------------------------------       ------------------------------//

                            string fileName = Guid.NewGuid().ToString().Replace("-", "") + ImageUpload.FileName.ToLower();
                            int FtpID = ftp.Upload("Content", fileName, ImageUpload.OpenReadStream());

                            if (FtpID != -1)
                            {
                                img.ServerId_FK = FtpID;
                                img.FileName = fileName;
                                img.Title = slider.SliderTitle;
                                img.Alt = slider.SliderTitle;
                                database.Update(img);
                                await database.SaveChangesAsync();
                            }

                        }
                        else
                        {
                            FtpWorker ftp = new FtpWorker();
                            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "Files\\Images\\Slider");

                            //-----------------Delete Image From Server && Local--------------------//

                            //if (slider.ServerUpload == true) //
                            //{
                            //    int FtpId = database.Tbl_Server.Where(c => c.Type == "Content").SingleOrDefault().ServerId;
                            //    var result = ftp.Remove(FtpId, tbl_slider.Tbl_Files.FileName);
                            //    if (result == false)
                            //        return RedirectToAction(nameof(Edit), new { id = tbl_slider.SliderId });
                            //}
                            //else
                            //{
                            //    var getFileName = Path.Combine(hostingEnvironment.WebRootPath, "Files\\Images\\Slider\\" + tbl_slider.Tbl_Files.FileName);
                            //    if (System.IO.File.Exists(getFileName))
                            //    {
                            //        System.IO.File.Delete(getFileName);
                            //    }
                            //}
                            //-----------------------------------     -----------------------------------------//

                            Files files = new Files();
                            if (ImageUpload != null && !string.IsNullOrEmpty(ImageUpload.FileName) && ImageUpload.Length > 0)
                            {
                                string fileName = Guid.NewGuid().ToString().Replace("-", "") + ImageUpload.FileName.ToLower();
                                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                                {
                                    await ImageUpload.CopyToAsync(fileStream);
                                }
                                int GetServerId = database.Tbl_Server.Where(c => c.Type == "Content").FirstOrDefault().ServerId;

                                files.ServerId_FK = GetServerId;
                                files.FileName = fileName;
                                files.Title = slider.SliderTitle;
                                files.Alt = slider.SliderTitle;
                                database.Tbl_Files.Add(files);

                                tbl_slider.FileId_FK = files.FileId;
                                await database.SaveChangesAsync();
                            }
                        }
                    }
                    tbl_slider.ServerUpload = slider.ServerUpload;
                    database.Update(tbl_slider);
                    await database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.SliderId))
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
            ViewData["FileId_FK"] = new SelectList(database.Tbl_Files, "FileId", "FileName", slider.FileId_FK);
            return View(slider);
        }

        // GET: Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await database.Tbl_Slider
                .Include(s => s.Tbl_Files)
                .SingleOrDefaultAsync(m => m.SliderId == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slider = await database.Tbl_Slider.SingleOrDefaultAsync(m => m.SliderId == id);
            database.Tbl_Slider.Remove(slider);
            await database.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return database.Tbl_Slider.Any(e => e.SliderId == id);
        }
    }
}
