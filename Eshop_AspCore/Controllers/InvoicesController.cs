using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using Eshop_AspCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop_AspCore.Controllers
{
    public class InvoicesController : Controller
    {
        public IActionResult Index(bool? statusPay)
        {
            ApplicationDbContext database = new ApplicationDbContext();

            List<Invoice> qInvoice = new List<Invoice>();
            if (statusPay == true)
            {
                qInvoice = database.Tbl_Invoice.Where(c => c.StatusPay == true).OrderByDescending(c => c.InvoiceId).ToList();
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
            else if (statusPay == false)
            {
                qInvoice = database.Tbl_Invoice.Where(c => c.StatusPay == false).OrderByDescending(c => c.InvoiceId).ToList();
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
            else
            {
                qInvoice = database.Tbl_Invoice.OrderByDescending(c => c.InvoiceId).ToList();

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

        }

        public IActionResult ShowDetailsOrders(int invoiceId)
        {
            ApplicationDbContext database = new ApplicationDbContext();
            try
            {
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
            catch
            {

                throw;
            }
        }
    }
}