using PaymentSystem.Common.Entities.Base;
using PaymentSystem.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Entities
{
    [Table("AccrualLoan", Schema = "PaymentSystem")]
    public class AccrualLoan : BaseApplicationEntity
    {
        public AccrualLoan()
        {
            this.Amount = (decimal)(new Random().Next(30, 70));
            this.ExpiryDate = DateUtilities.GetFirstDayOfNextMonth().AddDays((double)(new Random().Next(4, 19)));
            this.CollectionRecords = new List<CollectionRecord>();
            this.RefNumber = StringUtilities.GenerateKey(7).ToUpper();
        }

        public Guid MembershipId { get; set; }

        public User Membership { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string RefNumber { get; set; }

        public AccrualLoanType Type { get; set; }

        public virtual ICollection<CollectionRecord> CollectionRecords { get; set; }
    }
}

