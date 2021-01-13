using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using PaymentSystem.MVC.Hangfire.Job;
using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(PaymentSystem.MVC.Hangfire.App_Start.Startup))]

namespace PaymentSystem.MVC.Hangfire.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["PaymentSystemHangfire"].ConnectionString);

            RecurringJob.AddOrUpdate(() => Invoke(), "00 02 31 * *");
            RecurringJob.AddOrUpdate(() => Invoke(), "00 02 30 * *");
            RecurringJob.AddOrUpdate(() => Invoke(), "00 02 29 * *");
            RecurringJob.AddOrUpdate(() => Invoke(), "00 02 28 * *");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        public void Invoke()
        {
            using (var m = new ApiCallServiceJob())
            {
                m.DoWork();
            }
        }
    }
}
