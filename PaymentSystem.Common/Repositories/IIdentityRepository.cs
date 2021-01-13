using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Entities.Base;
using PaymentSystem.Common.Filters;
using PaymentSystem.Common.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Repositories
{
    public interface IIdentityRepository 
    {
        Task<IEnumerable<User>> GetAllUserAsync();

        IEnumerable<Role> GetAllRoles();

        User GetUserByUserNameOrEmail(string userNameOrEmail);

        User AddUser(User user);

        void UpdateUser(User user);

        IEnumerable<User> GetUserByFilter(UserFilter userFilter);

        User Validate(string userNameOrEmail, string password);
    }
}
