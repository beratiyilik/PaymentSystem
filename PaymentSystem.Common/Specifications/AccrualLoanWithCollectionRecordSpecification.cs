using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Specifications.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Common.Specifications
{
    public sealed class AccrualLoanWithCollectionRecordSpecification : BaseSpecification<AccrualLoan>
    {
        public AccrualLoanWithCollectionRecordSpecification() : base(null)
        {
            AddInclude(b => b.Membership);
            AddInclude(b => b.CollectionRecords);
        }

        public AccrualLoanWithCollectionRecordSpecification(Guid Id): base(b => b.Id == Id)
        {
            AddInclude(b => b.Membership);
            AddInclude(b => b.CollectionRecords);
        }
    }
}
