using PaymentSystem.Common.Entities.Base;
using PaymentSystem.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Entities
{
    [Table("CollectionRecord", Schema = "PaymentSystem")]
    public class CollectionRecord : BaseApplicationEntity
    {
        public CollectionRecord()
        {
            this.Amount = 0;
            this.RefNumber = StringUtilities.GenerateKey(7).ToUpper();
        }

        public decimal Amount { get; set; }

        public string RefNumber { get; set; }

        public Guid AccrualLoanId { get; set; }

        public AccrualLoan AccrualLoan { get; set; }

        public bool IsRefund { get; set; }
    }
}

