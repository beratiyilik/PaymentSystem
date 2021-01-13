using PaymentSystem.Application.Models;
using PaymentSystem.Common.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<IEnumerable<UserModel>> GetAllUserAsync();

        UserModel Validate(string userNameOrEmail, string password);

        UserModel RegisterMembership(UserModel userModel, Guid userId);

        void UnsubcribeMembership(UserModel userModel, Guid userId);

        IEnumerable<UserModel> GetMembershipByFilter(UserFilter filter);

        UserModel GetUserByUserNameOrEmail(string userNameOrEmail);
    }
}
