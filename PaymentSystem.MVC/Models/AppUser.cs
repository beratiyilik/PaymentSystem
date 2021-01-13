using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSystem.MVC.Models
{
    public class AppUser
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Roles")]
        public string[] Roles { get; set; }

        [JsonProperty("State")]
        public int State { get; set; }

        [JsonIgnore]
        public string FullName { get => $"{FirstName} {LastName}"; }

        [JsonProperty("IsManager")]
        public bool IsManager { get; set; }

        [JsonProperty("UserType")]
        public int UserType { get; set; }
    }
}