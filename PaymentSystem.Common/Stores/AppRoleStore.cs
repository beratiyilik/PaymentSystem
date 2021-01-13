using System;
using System.Threading.Tasks;
using PaymentSystem.Common.Entities;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace PaymentSystem.Common.Stores
{
    public class AppRoleStore : RoleStore<Role, Guid, UserRole>, IQueryableRoleStore<Role, Guid>, IRoleStore<Role, Guid>, IDisposable
    {
        public AppRoleStore() : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public AppRoleStore(DbContext context) : base(context)
        {

        }

        public new Task<Role> FindByIdAsync(Guid roleId)
        {
            throw new NotImplementedException();
        }
    }
}
