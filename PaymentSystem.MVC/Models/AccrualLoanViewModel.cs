using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.MVC.Models
{
    public class AccrualLoanViewModel
    {
        public AccrualLoanViewModel()
        {
            this.CollectionRecords = new List<CollectionRecordViewModel>();
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("State")]
        public int State { get; set; }

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        public UserViewModel Membership { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpiryDate { get; set; }

        public IEnumerable<CollectionRecordViewModel> CollectionRecords { get; set; }

        [JsonIgnore]
        public decimal RemainingLoan { get => this.Amount - this.CollectionRecords.Sum(m => m.Amount); }

        [JsonIgnore]
        public bool HasLoan { get => this.RemainingLoan > 0; }

        public string RefNumber { get; set; }

        public AccrualLoanType Type { get; set; }
    }
}
