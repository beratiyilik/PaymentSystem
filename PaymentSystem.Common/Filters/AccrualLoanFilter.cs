using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Filters
{
    public class AccrualLoanFilter
    {
        public Guid Id { get; set; }

        public Guid MembershipId { get; set; }

        public AccrualLoanType? Type { get; set; }
    }
}
