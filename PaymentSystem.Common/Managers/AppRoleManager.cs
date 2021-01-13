using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PaymentSystem.Common.Data;
using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Managers
{
    public class AppRoleManager : RoleManager<Role, Guid>
    {
        public AppRoleManager(IRoleStore<Role, Guid> roleStore) : base(roleStore)
        {

        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new AppRoleStore(context.Get<PaymentSystemContext>()));
        }
    }
}
