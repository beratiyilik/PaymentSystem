using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.MVC.Models
{
    public class CollectionRecordViewModel
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("State")]
        public int State { get; set; }
        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public Guid AccrualLoanId { get; set; }
        public AccrualLoanViewModel AccrualLoan { get; set; }
        public string RefNumber { get; set; }
        public bool IsRefund { get; set; }
    }
}
