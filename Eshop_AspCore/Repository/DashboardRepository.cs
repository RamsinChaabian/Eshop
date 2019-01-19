using Eshop_AspCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Repository
{
    public class DashboardRepository : IDisposable
    {
        private ApplicationDbContext database = null;

        public DashboardRepository()
        {
            database = new ApplicationDbContext();
        }
        public int getCountUsers()
        {
            var q = database.Users.Where(c => c.EmailConfirmed == true).Count();
            return q;
        }
        public int getConfirmCountOrders()
        {
            var q = database.Tbl_Invoice.Where(c => c.StatusPay == true).Count();
            return q;
        }
        public int getNotConfirmCountOrders()
        {
            var q = database.Tbl_Invoice.Where(c => c.StatusPay == false).Count();
            return q;
        }
        public int getCountProducts()
        {
            var q = database.Tbl_Products.Count();
            return q;
        }
        public int SeeProducts()
        {
            var q = database.Tbl_Products.Sum(c => c.SeeProduct);
            return q;
        }
        ~DashboardRepository()
        {
            Dispose();
        }
        public void Dispose()
        {

        }
    }
}
