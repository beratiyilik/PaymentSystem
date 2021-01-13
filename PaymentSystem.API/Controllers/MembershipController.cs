using AutoMapper;
using PaymentSystem.API.Controllers.Base;
using PaymentSystem.API.Filters;
using PaymentSystem.API.Models;
using PaymentSystem.Application.Interfaces;
using PaymentSystem.Application.Models;
using PaymentSystem.Common.Filters;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PaymentSystem.API.Controllers
{

    [Authorize(Roles = "Admin")]
    public class MembershipController : BaseApplicationApiController
    {
        private readonly IIdentityService _identityService;

        private readonly IPaymentSystemService _paymentSystemService;

        public MembershipController(IIdentityService identityService, IPaymentSystemService paymentSystemService)
        {
            _identityService = identityService;
            _paymentSystemService = paymentSystemService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RegisterMembership([FromBody] UserViewModel model)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserViewModel, UserModel>();
            });

            var userModel = config.CreateMapper().Map<UserViewModel, UserModel>(model);

            var membership = _identityService.RegisterMembership(userModel, CurrentUser.Id);

            return await this.ReturnSuccessModel(membership);
        }


        [HttpPost]
        public async Task<HttpResponseMessage> EnquiryMembership([FromBody] MembershipFilter filter)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<MembershipFilter, UserFilter>();
            });

            var filtered = config.CreateMapper().Map<MembershipFilter, UserFilter>(filter);

            var result = _identityService.GetMembershipByFilter(filtered);

            return await this.ReturnSuccessModel(result.Any() ? result : new UserModel[] { });
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UnsubcribeMembership([FromBody] UserViewModel model)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserViewModel, UserModel>();
            });
            var mapped = config.CreateMapper().Map<UserViewModel, UserModel>(model);
            _identityService.UnsubcribeMembership(mapped, CurrentUser.Id);
            return await this.ReturnSuccessModel();
        }
    }
}
