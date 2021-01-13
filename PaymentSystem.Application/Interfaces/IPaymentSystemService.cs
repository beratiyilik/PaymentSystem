using PaymentSystem.Application.Models;
using PaymentSystem.Common.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Interfaces
{
    public interface IPaymentSystemService
    {
        IEnumerable<AccrualLoanModel> GetAccrualLoans(AccrualLoanFilter filter);

        AccrualLoanModel Payment(AccrualLoanModel model);

        AccrualLoanModel GetAccrualLoanById(Guid id);

        void DepositRefund(UserModel model);

        UserModel GetMembershipPaymentInfo(Guid id);

        IEnumerable<UserModel> GetMembershipsWithPaymentInfo();
    }
}
