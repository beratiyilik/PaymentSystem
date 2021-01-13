using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Repositories;
using PaymentSystem.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using PaymentSystem.Common.Specifications;
using PaymentSystem.Common.Data;

namespace PaymentSystem.Infrastructure.Repository
{
    public class AccrualLoanRepository : Repository<AccrualLoan>, IAccrualLoanRepository
    {
        public AccrualLoanRepository(PaymentSystemContext dbContext) : base(dbContext)
        {

        }

        public IQueryable<AccrualLoan> GetAccrualLoans()
        {
            var spec = new AccrualLoanWithCollectionRecordSpecification();
            var testParent = Get(spec).AsQueryable();
            return testParent;
        }

        public User GetMembershipPaymentInfo(Guid id)
        {
            var query = GetMembershipsWithPaymentInfo().Where(m => m.Id == id);

            return query.FirstOrDefault();
        }

        public IQueryable<User> GetMembershipsWithPaymentInfo()
        {
            var query = _dbContext.Users
                .Include("AccrualLoans")
                .Include("AccrualLoans.CollectionRecords")
                .Where(m => m.State == Common.Enum.Enum.EntityState.Active && m.UserType == Common.Enum.Enum.UserType.Membership);

            return query;
        }

        public override AccrualLoan GetById(Guid id)
        {
            var query = GetAccrualLoans().Where(m => m.Id == id);
            return query.FirstOrDefault();
        }
    }
}
