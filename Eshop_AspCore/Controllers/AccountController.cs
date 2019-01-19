using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using Eshop_AspCore.Services;
using Eshop_AspCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eshop_AspCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger logger;
        private readonly IEmailSender emailSender;
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ILoggerFactory _loggerFactory, IEmailSender _emailSender)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            logger = _loggerFactory.CreateLogger<AccountController>();
            emailSender = _emailSender;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                var user = new ApplicationUser { Email = model.Email };
                var CheckeConfirmEmail = signInManager.UserManager.Users.Where(c => c.Email == model.Email).FirstOrDefault();
                if (result.Succeeded)
                {
                    if (CheckeConfirmEmail.EmailConfirmed == true)
                    {
                        logger.LogInformation(1, "کاربر وارد شد");
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "حساب کاربری شما فعال نمی باشد");
                        return View(model);
                    }
                }
                if (result.IsLockedOut)
                {
                    logger.LogWarning("حساب کاربری شما قفل می باشد ");
                }
                if (result.RequiresTwoFactor)
                {
                    //
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور اشتباه می باشد");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = model.Email, UserName = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                var role = database.Tbl_Role.Where(c => c.RoleName == "User").FirstOrDefault();

                if (result.Succeeded)
                {
                    database.Tbl_UserAccess.Add(new UserAccess
                    {
                        AddPost = false,
                        DeletePost = false,
                        EditPost = false,
                        RoleId_FK = role.RoleId,
                        UserId_FK = user.Id,
                    });
                    await database.SaveChangesAsync();

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await emailSender.SendEmailAsync(model.Email, "تایید اکانت کاربری",
                        $"لطفا روی لینک فعالسازی کلیک نمایید: <a href='{callbackUrl}'>لینک</a>");

                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //logger.LogInformation(3, "کاربر با موفقیت ثبت نام شد");
                    TempData["Style"] = "alert alert-success";
                    TempData["Message"] = "ثبت نام با موفقیت انجام شد ایمیلی حاوی لینک فعال سازی برای شما ارسال گردید";
                    return View(model);
                }
                AddErrors(result);
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userid, string code)
        {
            if (userid == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            //if(result.Succeeded)
            // {
            //     return View("Login");
            // }
            //else
            // {
            //     return View("Error");
            // }
            TempData["Style"] = "alert alert-success";
            TempData["MsgConfirmEmail"] = "حساب کاربری شما با موفقیت فعال گردید";
            return View(result.Succeeded ? "Login" : "Error");
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }
        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        [HttpGet]
        public IActionResult Profile()
        {
            ApplicationDbContext database = new ApplicationDbContext();

            string currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();
            if (qUser == null)
            {
                return Redirect(nameof(AccountController.Login));
            }
            VmUser vm = new VmUser();
            vm.UserId = qUser.Id;
            vm.Username = qUser.UserName;
            vm.Email = qUser.Email;
            vm.Mobile = qUser.Mobile == null ? null : qUser.Mobile;
            vm.FirstName = qUser.FirstName == null ? null : qUser.FirstName;
            vm.LastName = qUser.LastName == null ? null : qUser.LastName;

            return View(vm ?? null);
        }

        [HttpPost]
        public IActionResult RegisterProfile(VmUser u)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            string currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => (c.Id == u.UserId || c.UserName == currentUser) && c.EmailConfirmed == true).FirstOrDefault();

            if (qUser == null)
                return Redirect(nameof(AccountController.Profile));

            string CurrentMobile = u.Mobile;

            if (qUser.Mobile == CurrentMobile)
            {
                qUser.FirstName = u.FirstName;
                qUser.LastName = u.LastName;
                database.Users.Attach(qUser);
                database.Entry(qUser).State = EntityState.Modified;
                database.SaveChanges();

                return Redirect(nameof(AccountController.Profile));
            }
            else
            {
                bool isExistMob = database.Users.ToList().Exists(c => c.Mobile == u.Mobile);
                if (isExistMob == true)//اگر پیدا شد
                {
                    TempData["Style"] = "alert alert-warning text-center";
                    TempData["Message"] = "شماره موبایل وارد شده از قبل در سیستم وجود دارد";
                    return Redirect(nameof(AccountController.Profile));
                }

                else
                {
                    qUser.FirstName = u.FirstName;
                    qUser.LastName = u.LastName;
                    qUser.Mobile = u.Mobile;
                    database.Users.Attach(qUser);
                    database.Entry(qUser).State = EntityState.Modified;
                    database.SaveChanges();

                    return Redirect(nameof(AccountController.Profile));
                }
            }

        }
        public IActionResult PostInfo(string UserId = "")
        {
            ApplicationDbContext database = new ApplicationDbContext();

            string currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => (c.Id == UserId || c.UserName == currentUser) && c.EmailConfirmed == true).FirstOrDefault();

            VmUser vm = new VmUser();

            vm.Email = qUser.Email;
            vm.UserId = qUser.Id;
            vm.FirstName = qUser.FirstName;
            vm.LastName = qUser.LastName;
            vm.Address = qUser.Address == null ? null : qUser.Address;
            vm.City = qUser.City == null ? null : qUser.City;
            vm.Province = qUser.Province == null ? null : qUser.Province;
            vm.PostalCode = qUser.PostalCode == null ? null : qUser.PostalCode;

            return View(vm ?? null);
        }

        [HttpPost]
        public IActionResult RegisterUserPostInfo(VmUser u)
        {
            ApplicationDbContext database = new ApplicationDbContext();

            string currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser && c.EmailConfirmed == true && c.Id == u.UserId).FirstOrDefault();

            if (qUser == null)
            {
                return Redirect(nameof(AccountController.PostInfo));
            }

            qUser.PostalCode = u.PostalCode;
            qUser.City = u.City;
            qUser.Province = u.Province;
            qUser.Address = u.Address;

            database.Update(qUser);
            database.SaveChanges();

            return Redirect(nameof(AccountController.PostInfo));
        }

        public JsonResult CheckMobileExist(string Mobile)
        {
            string currentUser = User.Identity.Name;
            System.Threading.Thread.Sleep(200);
            ApplicationDbContext database = new ApplicationDbContext();

            var search = database.Users.Where(c => c.Mobile == Mobile && c.UserName != currentUser).SingleOrDefault();

            if (search != null)
            {
                return Json(1);//شماره موبایل از قبل وجود دارد
            }
            else
            {
                return Json(0);//وجود ندارد
            }
        }

        public IActionResult UserComments()
        {
            ApplicationDbContext database = new ApplicationDbContext();
            string currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser && c.EmailConfirmed == true).FirstOrDefault();
            if (qUser == null)
                return Redirect(nameof(AccountController.Profile));
            var qComments = database.Tbl_Comments.Where(c => c.UserId_FK == qUser.Id).OrderByDescending(c => c.CommentId).ToList();
            if (qComments.Count <= 0)
                return View();

            List<VmComments> lstComments = new List<VmComments>();

            foreach (var item in qComments)
            {
                VmComments vm = new VmComments();
                vm.CommentId = item.CommentId;
                vm.ConfirmComment = item.ConfirmComment;
                vm.DateComment = item.DateComment;
                vm.ProductId_FK = item.ProductId_FK;
                vm.Text = item.Text;
                vm.Title = item.Title;
                vm.UserId_FK = item.UserId_FK;
                var qGetProName = database.Tbl_Products.Where(c => c.ProductId == item.ProductId_FK).SingleOrDefault();
                vm.ProductName = qGetProName.ProductNameFA;
                lstComments.Add(vm);
            }
            return View(lstComments ?? null);
        }

        public IActionResult ListOrders()
        {
            ApplicationDbContext database = new ApplicationDbContext();
            string currentUser = User.Identity.Name;
            var qUser = database.Users.Where(c => c.UserName == currentUser && c.EmailConfirmed == true).FirstOrDefault();
            if (qUser == null)
                return Redirect(nameof(AccountController.Profile));

            var qInvoice = database.Tbl_Invoice.Where(c => c.UserId_FK == qUser.Id).OrderByDescending(c => c.InvoiceId).ToList();

            if (qInvoice.Count == 0)
                return View();

            List<VmPay> lstInvoice = new List<VmPay>();

            foreach (var item in qInvoice)
            {
                VmPay vm = new VmPay();
                vm.Authority = item.Authority;
                vm.DateOrder = item.DateOrder;
                vm.InvoiceId = item.InvoiceId;
                vm.RefID = item.RefID;
                vm.StatusPay = item.StatusPay;
                vm.SumPrice = item.SumPrice;
                vm.UserId_FK = item.UserId_FK;
                lstInvoice.Add(vm);

            }
            return View(lstInvoice ?? null);

        }

        public IActionResult ShowDetailsOrder(int invoiceId)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    string currentUser = User.Identity.Name;
                    var qUser = database.Users.Where(c => c.UserName == currentUser && c.EmailConfirmed == true).FirstOrDefault();
                    if (qUser == null)
                        return Redirect(nameof(AccountController.Profile));

                    var qShopping = database.Tbl_ShoppingCart.Where(c => c.InvoiceId == invoiceId)
                                                             .Include(c => c.Tbl_Products)
                                                             .ThenInclude(c => c.Tbl_Gallery)
                                                             .ToList();
                    var qInvoice = database.Tbl_Invoice.Where(c => c.InvoiceId == invoiceId).FirstOrDefault();

                    if (qShopping.Count > 0)
                    {
                        List<VmDetailsOrder> lstDetailsOrder = new List<VmDetailsOrder>();
                        foreach (var item in qShopping)
                        {
                            VmDetailsOrder vm = new VmDetailsOrder();
                            vm.ProductCount = item.ProductCount;
                            vm.InvoiceId = (int)item.InvoiceId;
                            vm.Date = item.DateShop;
                            vm.OffProduct = item.Tbl_Products.OffProduct;
                            vm.ProductId = item.ProductId_FK;
                            var PicName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId_FK
                                                                        && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                            vm.ProductImage = "/Files/Images/Products/" + PicName;
                            vm.ProductName = item.Tbl_Products.ProductNameFA;
                            vm.ProductPrice = item.Tbl_Products.Price;
                            vm.ShoppingId = item.ID;
                            vm.UserId = item.UserId_FK;

                            lstDetailsOrder.Add(vm);

                        }
                        ViewBag.InvoiceId = lstDetailsOrder[0].InvoiceId;
                        return View(lstDetailsOrder ?? null);
                    }
                    else
                    {
                        return View(null);
                    }
                }
                else
                {
                    return View(nameof(AccountController.Login));
                }

            }
            catch
            {

                throw;
            }

        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUser = User.Identity.Name;
                var user = await userManager.FindByNameAsync(currentUser);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, password);

                if (result.Succeeded)
                {
                    TempData["Style"] = "alert alert-success text-center";
                    TempData["Msg"] = "کلمه عبور با موفقیت ویرایش گردید";
                    return View();
                }
                else
                {
                    TempData["Style"] = "alert alert-danger text-center";
                    TempData["Msg"] = "خطایی رخ داده است لطفا مجددا تلاش نمایید";
                    return View();
                }
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
        }

        public IActionResult SendNewsLetter()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendNewsLetter(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                TempData["Style"] = "alert alert-danger";
                TempData["Msg"] = "لطفا متن خبرنامه را وارد نمایید";
                return View();
            }
            else
            {
                ApplicationDbContext database = new ApplicationDbContext();

                var qSettingSite = database.Tbl_SettingSite.FirstOrDefault();

                string[] numbers;
                var mobileNumbers = (from rows in database.Users where rows.EmailConfirmed == true select rows.Mobile).ToArray();
                numbers = mobileNumbers;
                var result = SmsService.SendSmsForNewsLetter(qSettingSite.SmsNumber, mobileNumbers, message);
                if (result)
                {
                    TempData["Style"] = "alert alert-success";
                    TempData["Msg"] = "خبرنامه با موفقیت برای کاربران ارسال گردید";
                    return View();
                }
                else
                {
                    TempData["Style"] = "alert alert-warning";
                    TempData["Msg"] = "خطایی در ارسال خبرنامه به وجود آمده است لطفا مجدد تلاش نمایید";
                    return View();
                }
            }
        }
    }
}