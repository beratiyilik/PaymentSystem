using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PaymentSystem.API.Providers;
using PaymentSystem.Common.Data;
using PaymentSystem.Common.Managers;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(PaymentSystem.API.App_Start.Startup))]

namespace PaymentSystem.API.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app.CreatePerOwinContext(PaymentSystemContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            ConfigureAuth(app);
        }
    }
}
