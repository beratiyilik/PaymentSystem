using PaymentSystem.Application.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Application.Models
{
    public class CollectionRecordModel : BaseModel
    {
        public decimal Amount { get; set; }

        public Guid AccrualLoanId { get; set; }

        public AccrualLoanModel AccrualLoan { get; set; }

        public string RefNumber { get; set; }

        public bool IsRefund { get; set; }
    }
}
