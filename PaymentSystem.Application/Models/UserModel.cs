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
    public class UserModel : BaseModel
    {
        public UserModel()
        {
            this.AccrualLoans = new List<AccrualLoanModel>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public string TaxNumber { get; set; }

        public UserType UserType { get; set; }

        public EntityType EntityType { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string[] Roles { get; set; }

        [JsonIgnore]
        public string FullName { get => $"{FirstName} {LastName}"; }

        public IEnumerable<AccrualLoanModel> AccrualLoans { get; set; }

        [JsonIgnore]
        public decimal RemainingLoan { get => this.AccrualLoans.Where(m => m.Type == AccrualLoanType.Invoice).Sum(m => m.RemainingLoan); }

        [JsonIgnore]
        public bool HasLoan { get => this.RemainingLoan > 0; }

        [JsonIgnore]
        public bool HasDepositRefunded
        {
            get
            {
                var accrualLoans = this.AccrualLoans.FirstOrDefault(m => m.Type == AccrualLoanType.Deposit);
                return accrualLoans != null && accrualLoans.CollectionRecords.Any(m => m.IsRefund) && accrualLoans.CollectionRecords.Sum(m => m.Amount) == 0;
            }
        }
    }
}
