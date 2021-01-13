using PaymentSystem.API.Controllers.Base;
using PaymentSystem.Application.Services;
using System.Net.Http;
using System.Web.Http;

namespace PaymentSystem.API.Controllers
{
    [Authorize(Roles = "Hangfire")]
    public class HangfireController : BaseApplicationApiController
    {
        private readonly BillingService _billingService;

        public HangfireController(BillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet]
        public HttpResponseMessage TriggerMonthlyBilling()
        {
            _billingService.TriggerMonthlyBilling(CurrentUser.UserData.Id);
            return  this.ReturnSuccessModel().Result;
        }
    }
}
