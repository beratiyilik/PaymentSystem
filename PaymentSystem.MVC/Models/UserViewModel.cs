using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.MVC.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            this.AccrualLoans = new List<AccrualLoanViewModel>();
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("State")]
        public int State { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("NationalIdentificationNumber")]
        public string NationalIdentificationNumber { get; set; }

        [JsonProperty("TaxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("UserType")]
        public UserType UserType { get; set; }

        [JsonProperty("EntityType")]
        public EntityType EntityType { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Roles")]
        public string[] Roles { get; set; }

        [JsonIgnore]
        public string FullName { get => $"{FirstName} {LastName}"; }

        public IEnumerable<AccrualLoanViewModel> AccrualLoans { get; set; }

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