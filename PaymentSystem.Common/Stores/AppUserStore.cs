using System;
using System.Threading.Tasks;
using PaymentSystem.Common.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace PaymentSystem.Common.Stores
{
    public class AppUserStore : UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>, IUserStore<User, Guid>, IDisposable
    {
        public AppUserStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public AppUserStore(DbContext context) : base(context)
        {

        }
    }
}
