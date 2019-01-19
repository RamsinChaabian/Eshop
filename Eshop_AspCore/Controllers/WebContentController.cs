using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Eshop_AspCore.Controllers
{
    public class WebContentController : Controller
    {
        private readonly IFileProvider fileProvider;
        public WebContentController(IFileProvider _fileProvider)
        {
            fileProvider = _fileProvider;
        }
        public IActionResult Index()
        {
            var contents = fileProvider.GetDirectoryContents("/wwwroot/Files/Images/");

            return View(contents);
        }
        public IActionResult Gallery()
        {
            var contents = fileProvider.GetDirectoryContents("/wwwroot/images/");
            return View(contents);
        }
    }
}