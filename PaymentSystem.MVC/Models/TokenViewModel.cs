using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSystem.MVC.Models
{
    public class TokenViewModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshTokenAsString { get; set; }

        [JsonIgnore]
        public Guid RefreshToken { get => !string.IsNullOrEmpty(RefreshTokenAsString) ? Guid.Parse(this.RefreshTokenAsString) : Guid.Empty; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("roles")]
        public string RolesAsString { get; set; }

        [JsonIgnore]
        public string[] Roles { get => !string.IsNullOrEmpty(this.RolesAsString) ? this.RolesAsString.Split(',') : new string[] { }; }

        [JsonProperty("userData")]
        public string UserDataAsString { get; set; }

        [JsonIgnore]
        public AppUser UserData { get => !string.IsNullOrEmpty(this.UserDataAsString) ? JsonConvert.DeserializeObject<AppUser>(this.UserDataAsString) : null; }

        [JsonProperty(".issued")]
        public string IssuedAsString { get; set; }

        [JsonIgnore]
        public DateTime Issued { get => !string.IsNullOrEmpty(this.IssuedAsString) ? DateTime.Parse(this.IssuedAsString) : default(DateTime); }

        [JsonProperty(".expires")]
        public string ExpiresAsString { get; set; }

        [JsonIgnore]
        public DateTime Expires { get => !string.IsNullOrEmpty(this.ExpiresAsString) ? DateTime.Parse(this.ExpiresAsString) : default(DateTime); }

        [JsonIgnore]
        public bool IsExpired { get => DateTime.UtcNow > this.Expires; }

        [JsonIgnore]
        public bool IsAdmin { get => this.Roles.Contains("Admin"); }

        [JsonIgnore]
        public bool IsManager { get => this.UserData != null && this.UserData.IsManager; }
    }

    public class TokenViewModel<T> : TokenViewModel
    {
        public new T UserData { get; set; }
    }
}