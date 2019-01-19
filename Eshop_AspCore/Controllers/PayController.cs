using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using Eshop_AspCore.Services;
using Eshop_AspCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop_AspCore.Controllers
{
    public class PayController : Controller
    {
        [HttpPost]
        public IActionResult AddToCart(string userId, int CountProduct, int productId = 0)
        {
            ApplicationDbContext database = new ApplicationDbContext();

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var qUser = database.Users.Where(c => c.Id == userId).FirstOrDefault();

                    var qProduct = database.Tbl_Products.Where(c => c.ProductId == productId).SingleOrDefault();

                    if (qUser == null && qProduct == null)
                        return RedirectToAction("Error", "Home");

                    var qShoppingCart = database.Tbl_ShoppingCart.Where(c => c.ProductId_FK == productId && c.UserId_FK == qUser.Id && c.Status == false).FirstOrDefault();

                    if (qShoppingCart != null)
                    {
                        qShoppingCart.ProductCount += CountProduct;
                        qShoppingCart.DateShop = DateTime.Now;

                        database.Tbl_ShoppingCart.Attach(qShoppingCart);
                        database.Entry(qShoppingCart).State = EntityState.Modified;
                        database.SaveChanges();

                    }
                    else
                    {
                        database.Tbl_ShoppingCart.Add(new ShoppingCart()
                        {
                            DateShop = DateTime.Now,
                            ProductCount = CountProduct,
                            ProductId_FK = qProduct.ProductId,
                            Status = false,
                            UserId_FK = qUser.Id,

                        });
                        database.SaveChanges();
                    }

                }
                else
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
            return Redirect(nameof(ShoppingCart));
        }

        public IActionResult ShoppingCart()
        {
            try
            {
                ApplicationDbContext database = new ApplicationDbContext();
                if (User.Identity.IsAuthenticated)
                {
                    string currentUser = User.Identity.Name;//get Username

                    var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();
                    if (qUser == null)
                        return null;

                    var qShoppingCart = database.Tbl_ShoppingCart.Where(c => c.UserId_FK == qUser.Id && c.Status == false && c.InvoiceId == null)
                                                                  .Include(c => c.Tbl_Products)
                                                                  .ThenInclude(c => c.Tbl_Gallery)
                                                                  .ToList();

                    List<VmShoppingCart> lstShopping = new List<VmShoppingCart>();

                    if (qShoppingCart.Count > 0)
                    {
                        foreach (var item in qShoppingCart)
                        {
                            VmShoppingCart vm = new VmShoppingCart();
                            vm.DateShop = item.DateShop;
                            vm.ID = item.ID;
                            vm.OffProduct = item.Tbl_Products.OffProduct;
                            vm.Price = item.Tbl_Products.Price;
                            vm.ProductCount = item.ProductCount;
                            vm.ProductId_FK = item.ProductId_FK;
                            vm.Status = false;
                            vm.ProductName = item.Tbl_Products.ProductNameFA;
                            vm.UserId_FK = item.UserId_FK;
                            vm.WeightPro = item.Tbl_Products.Weight;

                            string productImageName = database.Tbl_Gallery.Where(c => c.ProductId_FK == item.ProductId_FK && c.DefaultPicProduct == true).FirstOrDefault().PictureName;
                            vm.ProductImage = "/Files/Images/Products/" + productImageName;
                            if (item.Tbl_Products.OffProduct > 0 && item.Tbl_Products.OffProduct <= 100)
                            {
                                decimal p1 = Math.Ceiling(item.Tbl_Products.Price * item.Tbl_Products.OffProduct / 100);
                                decimal p2 = item.Tbl_Products.Price - p1;
                                decimal FinalPrice = (Math.Ceiling(p2 / 100) * 100);
                                vm.PriceWithOff = FinalPrice;
                            }
                            else
                            {
                                vm.PriceWithOff = item.Tbl_Products.Price;
                            }
                            lstShopping.Add(vm);
                        }
                        double Tax = database.Tbl_SettingSite.FirstOrDefault().Tax;
                        var sumPrice = lstShopping.Sum(c => c.PriceWithOff * c.ProductCount);
                        var sumWeight = lstShopping.Sum(c => c.WeightPro);
                        var qGetPriceByWeight = database.Tbl_Weight.Where(c => c.Weight_Min <= sumWeight && c.Weight_Max >= sumWeight).SingleOrDefault().Weight_Price;

                        ViewBag.Maliat = Tax;
                        ViewBag.PostagePrice = qGetPriceByWeight;

                        ViewBag.SumPrice = sumPrice;
                        ViewBag.SumWeight = sumWeight;


                    }
                    return View(lstShopping ?? null);

                }
                else
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Error");
            }


        }
        [HttpPost]
        public IActionResult RemoveProductOfShoppingCart(int shoppingId)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                string currentUser = User.Identity.Name;//get Username

                var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();
                if (qUser == null)
                    return null;

                var qShoppingCart = database.Tbl_ShoppingCart.Where(c => c.UserId_FK == qUser.Id && c.Status == false && c.ID == shoppingId).SingleOrDefault();

                if (qShoppingCart == null)
                    return Redirect(nameof(ShoppingCart));

                database.Tbl_ShoppingCart.Remove(qShoppingCart);
                database.SaveChanges();
                return Redirect(nameof(ShoppingCart));
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
        }
        public IActionResult RemoveAllProductOfShoppingCart()
        {
            ApplicationDbContext database = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                string currentUser = User.Identity.Name;//get Username

                var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();
                if (qUser == null)
                    return null;

                var qShoppingCart = database.Tbl_ShoppingCart.Where(c => c.UserId_FK == qUser.Id && c.Status == false).ToList();

                if (qShoppingCart == null)
                    return Redirect(nameof(ShoppingCart));

                foreach (var item in qShoppingCart)
                {
                    database.Tbl_ShoppingCart.Remove(item);
                    database.SaveChanges();
                }
                return Redirect(nameof(ShoppingCart));
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
        }
        public IActionResult CheckoutSendType(decimal FinalPrice)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                string currentUser = User.Identity.Name;//get Username

                var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();
                if (qUser == null)
                    return null;
                if (string.IsNullOrEmpty(qUser.FirstName) || string.IsNullOrEmpty(qUser.LastName) || string.IsNullOrEmpty(qUser.Mobile))
                {
                    TempData["Style"] = "alert alert-warning text-center";
                    TempData["Msg"] = "جهت ادامه خرید شما باید اطلاعات کاربری خود را تکمیل نمایید";
                    return RedirectToAction(nameof(AccountController.Profile), "Account");
                }
                if (string.IsNullOrEmpty(qUser.PostalCode) || string.IsNullOrEmpty(qUser.City) || string.IsNullOrEmpty(qUser.Address))
                {
                    TempData["Style"] = "alert alert-warning text-center";
                    TempData["Msg"] = "جهت ادامه خرید شما باید اطلاعات محل سکونت خود را تکمیل نمایید";

                    return RedirectToAction(nameof(AccountController.PostInfo), "Account");
                }

                VmUser vm = new VmUser();
                vm.Address = qUser.Address;
                vm.City = qUser.City;
                vm.Email = qUser.Email;
                vm.FirstName = qUser.FirstName;
                vm.LastName = qUser.LastName;
                vm.Mobile = qUser.Mobile;
                vm.PhoneNumber = qUser.PhoneNumber;
                vm.PostalCode = qUser.PostalCode;
                vm.Province = qUser.Province;
                vm.UserId = qUser.Id;
                vm.Username = qUser.UserName;

                ViewBag.FinalPrice = FinalPrice;
                return View(vm);
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
        }
        public IActionResult FinalCheckout(decimal FinalPrice)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                string currentUser = User.Identity.Name;//get Username

                var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();
                if (qUser == null)
                    return null;

                string transactionId = "";
                if (database.Tbl_Invoice.Where(c => c.UserId_FK == qUser.Id && c.StatusPay == false).FirstOrDefault() == null)
                {
                    transactionId = Guid.NewGuid().ToString().Replace("-", "").ToLower();

                    database.Tbl_Invoice.Add(new Invoice
                    {
                        DateOrder = DateTime.Now,
                        StatusPay = false,
                        SumPrice = FinalPrice,
                        UserId_FK = qUser.Id,
                        TransactionId = transactionId,
                    });
                    database.SaveChanges();

                    TempData["Style"] = "alert alert-success text-center";
                    TempData["Message"] = "فاکتور شما با شماره : " + "[" + transactionId + "]" + " قایل پیگیری می باشد";
                    ViewBag.TransactionId = transactionId;

                }
                else
                {
                    var qInvoice = database.Tbl_Invoice.Where(c => c.UserId_FK == qUser.Id && c.StatusPay == false).FirstOrDefault();

                    qInvoice.DateOrder = DateTime.Now;
                    qInvoice.SumPrice = FinalPrice;
                    database.Tbl_Invoice.Attach(qInvoice);
                    database.Entry(qInvoice).State = EntityState.Modified;
                    database.SaveChanges();

                    TempData["Style"] = "alert alert-success text-center";
                    TempData["Message"] = "فاکتور شما با شماره : " + "[" + qInvoice.TransactionId + "]" + " قایل پیگیری می باشد";
                    ViewBag.TransactionId = qInvoice.TransactionId;
                }

                ViewBag.FinalPrice = FinalPrice;
                ViewBag.UserID = qUser.Id;

                return View();

            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");

            }
        }
        public async Task<IActionResult> Payment(string TransactionID, string UserId, decimal FinalPrice)
        {
            ApplicationDbContext database = new ApplicationDbContext();

            if (User.Identity.IsAuthenticated)
            {
                string currentUser = User.Identity.Name;//get Username

                var qUser = database.Users.Where(c => c.UserName == currentUser).FirstOrDefault();

                if (qUser == null)
                    return null;

                if (FinalPrice == 0 && TransactionID == string.Empty)
                    return RedirectToAction(nameof(HomeController.Error), "Home");

                var qInvoice = database.Tbl_Invoice.Where(c => c.UserId_FK == UserId && c.TransactionId == TransactionID
                                                            && c.SumPrice == FinalPrice && c.StatusPay == false).FirstOrDefault();

                if (qInvoice == null)
                    return null;

                int price;
                int.TryParse(FinalPrice.ToString(), out price);

                string description = "پرداخت فاکتور شماره : " + qInvoice.InvoiceId;

                var pay = await new ZarinpalSandbox.Payment(price).PaymentRequest(description, "https://localhost:44340" + Url.Action("PaymentVerify", new { UserID = qUser.Id }), "Ramsin.chaabian@gmail.com", "09363608399");


                if (pay.Status == 100)//اگر پرداخت موفقیت آمیز بود
                {
                    qInvoice.Authority = pay.Authority;
                    qInvoice.DescriptionPay = description;
                    database.Tbl_Invoice.Update(qInvoice);
                    database.SaveChanges();



                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + pay.Authority);
                }
                else
                {
                    return RedirectToAction("Error", new { Code = pay.Status });
                }
            }

            else
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");

            }

        }
        public async Task<IActionResult> PaymentVerify(VmPayVerify vm, string UserID = "")
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
            if (vm.Status == "OK")
            {
                ApplicationDbContext database = new ApplicationDbContext();
                var qFactor = database.Tbl_Invoice.Where(c => c.Authority == vm.Authority && c.StatusPay == false).SingleOrDefault();
                int Amount;
                //Amount = (int)qFactor.SumPrice;
                int.TryParse(qFactor.SumPrice.ToString(),NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,out Amount);
                var vertification = await new ZarinpalSandbox.Payment(Amount).Verification(vm.Authority);
                if (vertification.Status == 100)
                {
                    qFactor.StatusPay = true;
                    qFactor.RefID = vertification.RefId;
                    database.Update(qFactor);
                    database.SaveChanges();

                    var qShopping = database.Tbl_ShoppingCart.Where(c => c.UserId_FK == qFactor.UserId_FK &&
                                                                      c.Status == false).ToList();

                    foreach (var item in qShopping)
                    {
                        item.Status = true;
                        item.InvoiceId = qFactor.InvoiceId;
                        database.Update(item);
                        database.SaveChanges();
                    }

                    var q = database.Tbl_SettingSite.FirstOrDefault();

                    var qUser = database.Users.Where(c => c.Id == UserID && c.EmailConfirmed == true).FirstOrDefault();

                    string message = qUser.FirstName + " " + qUser.LastName + " " + "گرامی" + Environment.NewLine + "پرداخت شما با موفقیت انجام شد." + Environment.NewLine + "شماره فاکتور شما : " + qFactor.InvoiceId;
                    SmsService.SendSms(q.SmsNumber, qUser.Mobile, message);

                    return RedirectToAction(nameof(PayController.PayResult), "Pay", new { factorId = qFactor.InvoiceId });
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");

                }
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        public IActionResult PayResult(int FactorId = 0)
        {
            if (FactorId != 0)
            {
                TempData["Message"] = "پرداخت شما با موفقیت انجام شد شماره فاکتور شما : " + FactorId;
            }
            return View();
        }

        public IActionResult Error(int code)
        {
            switch (code)
            {
                case 101:
                    {
                        ViewBag.PaymentMsg = "پرداخت قبلا انجام شده ";
                        break;
                    }
                default:
                    break;
            }
            return View();
        }
    }
}