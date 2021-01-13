using AutoMapper;
using Newtonsoft.Json;
using PaymentSystem.API.Controllers.Base;
using PaymentSystem.API.Models;
using PaymentSystem.Application.Interfaces;
using PaymentSystem.Application.Models;
using PaymentSystem.Common.Filters;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.API.Controllers
{
    [Authorize(Roles = "Admin, Membership")]
    public class PaymentSystemController : BaseApplicationApiController
    {
        private readonly IIdentityService _identityService;

        private readonly IPaymentSystemService _paymentSystemService;

        public PaymentSystemController(IIdentityService identityService, IPaymentSystemService paymentSystemService)
        {
            _identityService = identityService;
            _paymentSystemService = paymentSystemService;
        }


        public async Task<HttpResponseMessage> GetAccrualLoans()
        {
            AccrualLoanFilter filter = CurrentUser.UserData.UserType == Common.Enum.Enum.UserType.Membership ? new AccrualLoanFilter { MembershipId = CurrentUser.UserData.Id, Type = Common.Enum.Enum.AccrualLoanType.Invoice } : null;

            var result = _paymentSystemService.GetAccrualLoans(filter);

            return await this.ReturnSuccessModel(result.Any() ? result : new AccrualLoanModel[] { });
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Payment([FromBody] AccrualLoanModel model)
        {
            if (CurrentUser.UserData.UserType == Common.Enum.Enum.UserType.Membership)
            {
                var orginalModel = _paymentSystemService.GetAccrualLoanById(model.Id);

                if (orginalModel.Membership.Id != CurrentUser.UserData.Id)
                {
                    var responseModel = new BaseResponseModel
                    {
                        ReturnCode = (int)Active.False,
                        Message = "You cannot pay another member's bill!",
                    };

                    var dataAsJson = JsonConvert.SerializeObject(responseModel, Formatting.None,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

                    var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
                    response.Content = new StringContent(dataAsJson, Encoding.UTF8, "application/json");

                    return response;
                }
            }

            model.CreatedBy = CurrentUser.Id;
            var result = _paymentSystemService.Payment(model);
            return await this.ReturnSuccessModel(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<HttpResponseMessage> DepositRefund([FromBody] UserViewModel model)
        {
            model.CreatedBy = CurrentUser.Id;
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserViewModel, UserModel>();
            });
            var mapped = config.CreateMapper().Map<UserViewModel, UserModel>(model);
            _paymentSystemService.DepositRefund(mapped);
            return await this.ReturnSuccessModel();
        }
    }
}
