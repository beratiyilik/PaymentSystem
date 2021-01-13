using Unity;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using PaymentSystem.Common.Managers;
using PaymentSystem.Common.Exceptions;
using PaymentSystem.Common.Entities;
using PaymentSystem.Application.Services;
using PaymentSystem.Application.Models;
using PaymentSystem.Application.Interfaces;
using Newtonsoft.Json;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.Owin;
using static PaymentSystem.Common.Enum.Enum;
using PaymentSystem.API.App_Start;

namespace PaymentSystem.API.Providers
{
    public class OAuthCustomeTokenProvider : OAuthAuthorizationServerProvider
    {
        private static IIdentityService _identityService;

        private readonly string _publicClientId;

        public OAuthCustomeTokenProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var userManager = context.OwinContext.GetUserManager<AppUserManager>();

                User user = await userManager.FindAsync(context.UserName, context.Password);

                IFormCollection parameters = await context.Request.ReadFormAsync();
                var app_type = parameters["app_type"] != null ? Int32.Parse(parameters["app_type"]) : -1;

                if (user == null || (int)user.UserType != app_type)
                {
                    context.SetError("invalid_grant", "Either username(email) or password is incorrect");
                    return;
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, Startup.OAuthOptions.AuthenticationType);
                // ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);

                context.Validated(ticket);
                // context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (IncorrectLoginAttemptException)
            {
                context.SetError("invalid_grant", "Either username(email) or password is incorrect");
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            _identityService = UnityConfig.Container.Resolve<IIdentityService>();

            var user = _identityService.GetUserByUserNameOrEmail(userName);

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName },
                { "roles", string.Join(",", user.Roles) },
                { "userData",  JsonConvert.SerializeObject(user) }
            };

            return new AuthenticationProperties(data);
        }
    }
}