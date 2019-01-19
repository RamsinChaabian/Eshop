using Eshop_AspCore.Data;
using Eshop_AspCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop_AspCore.Repository
{
    public class UserRepository : IDisposable
    {
        ApplicationDbContext database = null;
        public UserRepository()
        {
            database = new ApplicationDbContext();

        }

        public ApplicationUser GetUserById(string UserId)
        {
            if (UserId == null)
                return null;

            var qUser = database.Users.Where(c => c.Id == UserId).SingleOrDefault();

            return qUser ?? null;
        }

        ~UserRepository()
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
