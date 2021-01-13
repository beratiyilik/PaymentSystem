using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PaymentSystem.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PaymentSystem.API.Models
{
    public class CurrentUserModel : ClaimsPrincipal
    {
        public CurrentUserModel(ClaimsPrincipal principal): base(principal)
        {

        }

        public Guid Id { get => Guid.Parse(this.FindFirst(ClaimTypes.Sid).Value); }

        public string UserName => this.FindFirst(ClaimTypes.Name).Value;

        public string Email => this.FindFirst(ClaimTypes.Email).Value;

        public IEnumerable<string> Roles { get => this.FindAll(ClaimTypes.Role).Select(m => m.Value); }

        public UserModel UserData => JsonConvert.DeserializeObject<UserModel>(this.FindFirst(ClaimTypes.UserData).Value);

        public bool IsAdmin => this.Roles.Contains("Admin");
    }
}
