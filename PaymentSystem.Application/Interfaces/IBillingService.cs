using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Interfaces
{
    public interface IBillingService
    {
        void TriggerMonthlyBilling(Guid userId);
    }
}
