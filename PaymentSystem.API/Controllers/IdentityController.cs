using AutoMapper;
using PaymentSystem.API.Controllers.Base;
using PaymentSystem.API.Models;
using PaymentSystem.Application.Interfaces;
using PaymentSystem.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PaymentSystem.API.Controllers
{

    [Authorize(Roles = "Admin")]
    public class IdentityController : BaseApplicationApiController
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        //GET api/identity
        public async Task<HttpResponseMessage> GetAllUser()
        {
            var user = CurrentUser;

            var users = (await _identityService.GetAllUserAsync());

            return await this.ReturnSuccessModel(users);
        }
    }
}
