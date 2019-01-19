using Eshop_AspCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Eshop_AspCore.ViewModels.VmDetailsProduct;

namespace Eshop_AspCore.Repository
{
    public class CommentRepository : IDisposable
    {
        ApplicationDbContext database = null;

        public CommentRepository()
        {
            database = new ApplicationDbContext();
        }

        public List<ProductComments> ShowListComments(int ProductId)
        {
            try
            {
                var qComment = database.Tbl_Comments.Where(c => c.ProductId_FK == ProductId &&
                                                    c.ConfirmComment == true)
                                                    .OrderByDescending(c => c.CommentId).ToList();

                if (qComment == null)
                    return null;
                List<ProductComments> lstComments = new List<ProductComments>();
                foreach (var item in qComment)
                {
                    if (item != null)
                    {
                        UserRepository Rep_User = new UserRepository();
                        var qUserId = Rep_User.GetUserById(item.UserId_FK);

                        ProductComments vm = new ProductComments(); // => Is ViewModel

                        vm.CommentId = item.CommentId;
                        vm.DateComment = item.DateComment;
                        vm.FulllName = qUserId.FirstName + " " + qUserId.LastName;
                        vm.Email = item.Email;
                        vm.ProductId_FK = item.ProductId_FK;
                        vm.Text = item.Text;
                        vm.Title = item.Title;
                        vm.UserId_FK = qUserId.Id;

                        lstComments.Add(vm);

                    }
                }
                return lstComments ?? null;
            }
            catch
            {
                throw;
            }
        }

        public int GetCountUnConfirmComments(string user)//for User
        {
            try
            {
                var qUserid = database.Users.Where(c => c.UserName == user).SingleOrDefault().Id;
                if (qUserid == null)
                    return 0;
                var qComment = database.Tbl_Comments.Where(c => c.UserId_FK == qUserid && c.ConfirmComment == false).Count();
                if (qComment == 0)
                    return 0;
                else
                    return qComment;
            }
            catch
            {

                return 0;
            }
        }
        public int GetCountAllUnConfirmComments()//for Admin
        {
            
            try
            {
                var qComment = database.Tbl_Comments.Where(c => c.ConfirmComment == false).Count();
                if (qComment == 0)
                    return 0;
                else
                    return qComment;
            }
            catch
            {

                return 0;
            }
        }
        ~CommentRepository()
        {
            Dispose(true);
        }
        public void Dispose()
        {

        }
        public void Dispose(bool isDispose)
        {
            if (isDispose)
            {
                Dispose();
            }
        }
    }
}
