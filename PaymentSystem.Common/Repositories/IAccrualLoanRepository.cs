using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Repositories
{
    public interface IAccrualLoanRepository : IRepository<AccrualLoan>
    {
        IQueryable<AccrualLoan> GetAccrualLoans();
        IQueryable<User> GetMembershipsWithPaymentInfo();
        User GetMembershipPaymentInfo(Guid id);
    }
}
