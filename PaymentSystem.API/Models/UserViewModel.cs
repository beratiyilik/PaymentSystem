using Newtonsoft.Json;
using PaymentSystem.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.API.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            this.AccrualLoans = new List<AccrualLoanModel>();
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

        public IEnumerable<AccrualLoanModel> AccrualLoans { get; set; }

        [JsonIgnore]
        public decimal RemainingLoan { get => this.AccrualLoans.Sum(m => m.RemainingLoan); }

        [JsonIgnore]
        public bool HasLoan { get => this.RemainingLoan > 0; }

        public Guid CreatedBy { get; set; }
    }
}