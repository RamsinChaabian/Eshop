using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Eshop_AspCore.Data;
using Eshop_AspCore.ViewModels;

namespace Eshop_AspCore.Repository
{
    public class MenuRepository : IDisposable
    {
         ApplicationDbContext database;

        public MenuRepository()
        {
            database = new ApplicationDbContext();
        }

        public List<VmMenu> GetMenu()
        {
            try
            {
                var qMenu = (from rows in database.Tbl_Menu
                             where rows.IsActive == true
                             select new VmMenu
                             {
                                 ID = rows.ID,
                                 Title = rows.Title,
                                 Link = rows.Link
                             }).ToList();

                return qMenu;
            }
            catch
            {
                throw;
            }
        }

        ~MenuRepository()
        {
            Dispose();
        }

        public void Dispose()
        {

        }
    }
}
