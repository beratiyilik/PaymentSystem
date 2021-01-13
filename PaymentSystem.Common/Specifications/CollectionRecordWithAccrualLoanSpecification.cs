using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Specifications.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Common.Specifications
{
    public sealed class CollectionRecordWithAccrualLoanSpecification : BaseSpecification<CollectionRecord>
    {
        /*
        public CollectionRecordWithAccrualLoanSpecification(string name)
            : base(p => p.Name.ToLower().Contains(name.ToLower()))
        {
            AddInclude(p => p.Parent);
        }
        */

        public CollectionRecordWithAccrualLoanSpecification() : base(null)
        {
            AddInclude(p => p.AccrualLoan);
            AddInclude(p => p.AccrualLoan.Membership);
        }
    }
}
