using Eshop_AspCore.Data;
using Eshop_AspCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Repository
{
    public class CategoryRepository:IDisposable
    {
        private ApplicationDbContext database = null;
        public CategoryRepository()
        {
            database = new ApplicationDbContext();
        }


        public List<VmCategory> ShowCategories() // دسته بندی اصلی محصولات
        {
            try
            {
                var qCat = (from rows in database.Tbl_Category
                            select new VmCategory
                            {
                                CatId = rows.CatId,
                                CatTitle = rows.CatTitle,
                                Link = rows.CatId + "-" + rows.CatTitle
                            }).ToList();
                return qCat;
            }
            catch
            {

                throw;
            }
        }

        public List<VmFirstSubCategory> ShowFirstSubCategory()
        {
            try
            {
                var q = (from a in database.Tbl_FirstSubCategory
                         join b in database.Tbl_Category on a.CatId_FK equals b.CatId
                         select new VmFirstSubCategory
                         {
                             FirstSubCatId = a.FirstSubCatId,
                             CatId_FK = b.CatId,
                             FirstSubCatTitle = a.FirstSubCatTitle,
                             Link = a.FirstSubCatId + "-" + a.FirstSubCatTitle,
                             Picture = "/Files/Images/" + a.Picture,
                             CountProduct = database.Tbl_Products.Count(c => c.FirstSubCat_FK == a.FirstSubCatId),

                             

                         }).ToList();
                return q;
            }
            catch
            {

                throw;
            }
        }

        public List<VmFirstSubCategory> ShowArchiveFirstSubCategory()
        {
            try
            {
                var q = (from a in database.Tbl_FirstSubCategory
                         join b in database.Tbl_Category on a.CatId_FK equals b.CatId
                         join d in database.Tbl_Products on a.FirstSubCatId equals d.FirstSubCat_FK
                         select new VmFirstSubCategory
                         {
                             FirstSubCatId = a.FirstSubCatId,
                             CatId_FK = b.CatId,
                             FirstSubCatTitle = a.FirstSubCatTitle,
                             Link = a.FirstSubCatId + "-" + a.FirstSubCatTitle,
                             Picture = "/Files/Images/" + a.Picture,
                             CountProduct = database.Tbl_Products.Count(c => c.FirstSubCat_FK == a.FirstSubCatId),

                         }).Distinct().ToList();
                return q;
            }
            catch
            {

                throw;
            }
        }

        public List<VmSecondSubCategory> ShowSecondSubCategory()
        {
            try
            {
                var q = (from a in database.Tbl_SecondSubCategory
                         join b in database.Tbl_FirstSubCategory on a.FirstSubCatId_FK equals b.FirstSubCatId
                         select new VmSecondSubCategory
                         {
                             SecondSubCatId = a.SecondSubCatId,
                             FirstSubCatId_FK = b.FirstSubCatId,
                             SecondSubCatTitle = a.SecondSubCatTitle,
                             Link = a.SecondSubCatId + "-" + a.SecondSubCatTitle
                         }).ToList();
                return q;
            }
            catch
            {
                throw;
            }
        }

        public List<VmLastSubCategory> ShowLastSubCategory()
        {
            try
            {
                var q = (from a in database.Tbl_LastSubCategory
                         join b in database.Tbl_SecondSubCategory on a.SecondSubCatId_FK equals b.SecondSubCatId
                         select new VmLastSubCategory
                         {
                             LastSubCatId = a.LastSubCatId,
                             SecondSubCatId_FK = b.SecondSubCatId,
                             LastSubCatTitle = a.LastSubCatTitle,
                             Link = a.LastSubCatId + "-" + a.LastSubCatTitle
                         }).ToList();
                return q;

            }
            catch
            {

                throw;
            }
        }

        ~CategoryRepository()
        {
            Dispose();
        }

        public void Dispose()
        {

        }
    }
}
