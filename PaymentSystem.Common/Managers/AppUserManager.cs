using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PaymentSystem.Common.Data;
using PaymentSystem.Common.Entities;
using PaymentSystem.Common.MessageServices;
using PaymentSystem.Common.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Managers
{
    public class AppUserManager : UserManager<User, Guid>
    {
        public AppUserManager(IUserStore<User, Guid> store) : base(store)
        {

        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(new UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>(context.Get<PaymentSystemContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User, Guid>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User, Guid>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EMailService();

            manager.SmsService = new SMSService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            var claimsIdentity = await base.CreateIdentityAsync(user, authenticationType);

            if (!claimsIdentity.HasClaim(m => m.Type == ClaimTypes.Sid && m.Value == Convert.ToString(user.Id)))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, Convert.ToString(user.Id)));


            if (!claimsIdentity.HasClaim(m => m.Type == ClaimTypes.Name && m.Value == user.UserName))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            if (!claimsIdentity.HasClaim(m => m.Type == ClaimTypes.Email && m.Value == user.Email))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            // TODO: roles
            var roles = this.GetRoles(user.Id);

            var userDataAsString = JsonConvert.SerializeObject(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.NationalIdentificationNumber,
                user.TaxNumber,
                user.UserType,
                user.EntityType,
                user.Email,
                user.UserName,
                user.CreatedAt,
                user.CreatedById,
                user.State,
                Roles = roles.Select(m => m).ToArray()
            });
            if (!claimsIdentity.HasClaim(m => m.Type == ClaimTypes.UserData && m.Value == userDataAsString))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, userDataAsString));

            return claimsIdentity;
        }
    }
}
