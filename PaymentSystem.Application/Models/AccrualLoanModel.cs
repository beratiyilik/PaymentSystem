using Newtonsoft.Json;
using PaymentSystem.Application.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Application.Models
{
    public class AccrualLoanModel : BaseModel
    {
        public AccrualLoanModel()
        {
            this.CollectionRecords = new List<CollectionRecordModel>();
        }

        public UserModel Membership { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpiryDate { get; set; }

        public IEnumerable<CollectionRecordModel> CollectionRecords { get; set; }

        [JsonIgnore]
        public decimal RemainingLoan { get => this.Amount - this.CollectionRecords.Sum(m => m.Amount); }

        [JsonIgnore]
        public bool HasLoan { get => this.RemainingLoan > 0; }

        public string RefNumber { get; set; }

        public AccrualLoanType Type { get; set; }
    }
}
