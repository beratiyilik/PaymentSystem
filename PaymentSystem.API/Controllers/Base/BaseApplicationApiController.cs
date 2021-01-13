using Newtonsoft.Json;
using PaymentSystem.API.Models;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.API.Controllers.Base
{
    public abstract class BaseApplicationApiController : ApiController
    {
        protected JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings() 
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        protected async Task<HttpResponseMessage> ReturnSuccessModel(object data = null)
        {
            var responseModel = new BaseResponseModel
            {
                ReturnCode = (int)Active.True,
                ErrorCode = null,
                Message = "Has completed successfully",
                ResultData = data,
            };

            var dataAsJson = JsonConvert.SerializeObject(responseModel, Formatting.None, JsonSerializerSettings);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
            return response;
        }

        protected async Task<HttpResponseMessage> ReturnPrivateErrorModel(ErrorCode errorCode, string message)
        {
            var responseModel = new BaseResponseModel
            {
                ReturnCode = (int)Active.False,
                ErrorCode = (int)errorCode,
                Message = message
            };

            var dataAsJson = JsonConvert.SerializeObject(responseModel, Formatting.None, JsonSerializerSettings);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
            return response;
        }

        protected async Task<HttpResponseMessage> ReturnValidationErrorModel(string message = null)
        {
            var responseModel = new BaseResponseModel
            {
                ReturnCode = (int)Active.False,
                ErrorCode = (int)ErrorCode.ValidError,
                Message = string.IsNullOrEmpty(message) ? "Please fill all required fields." : message
            };

            var dataAsJson = JsonConvert.SerializeObject(responseModel, Formatting.None, JsonSerializerSettings);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
            return response;
        }

        protected async Task<HttpResponseMessage> ReturnItemNotFoundModel(string item = null)
        {
            var responseModel = new BaseResponseModel
            {
                ReturnCode = (int)Active.False,
                ErrorCode = (int)ErrorCode.ItemNotFound,
                Message = string.IsNullOrEmpty(item) ? "Item was not found." : $"{item}"
            };

            var dataAsJson = JsonConvert.SerializeObject(responseModel, Formatting.None, JsonSerializerSettings);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
            return response;
        }

        protected async Task<HttpResponseMessage> ReturnUnknownErrorModel(string message = null)
        {
            var responseModel = new BaseResponseModel
            {
                ReturnCode = (int)Active.False,
                ErrorCode = (int)ErrorCode.UnknownError,
                Message = string.IsNullOrEmpty(message) ? "Unknown Error" : message,
            };

            var dataAsJson = JsonConvert.SerializeObject(responseModel, Formatting.None, JsonSerializerSettings);
            var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            response.Content = new StringContent(dataAsJson, Encoding.UTF8, "application/json");
            return response;
        }

        protected CurrentUserModel CurrentUser => new CurrentUserModel(this.User as ClaimsPrincipal);
    }
}
