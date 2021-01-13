using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PaymentSystem.MVC.Models;
using PaymentSystem.MVC.ConstantValues;
using System;
using System.Net.Http.Headers;
using System.Text;
using PaymentSystem.MVC.Models.Base;

namespace PaymentSystem.MVC.Controllers.Base
{
    public abstract class BaseApplicationController : Controller
    {
        private static string _baseUrlAsString = "https://localhost:44399";

        private static Uri _baseUrl = new Uri(_baseUrlAsString);

        protected JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        protected JsonSerializer JsonSerializer = new JsonSerializer();

        protected virtual string SESSION_KEY { get => Keys.DEFAULT_SESSION_KEY; }

        protected HttpResponseMessage PostAPIRequest(string endpoint, object model, bool addAuthorization = true)
        {
            HttpResponseMessage response = null;

            using (var client = CreateHttpClient(addAuthorization))
            {
                var objectAsJSON = JsonConvert.SerializeObject(model);

                var content = new StringContent(objectAsJSON, Encoding.UTF8, "application/json");

                response = client.PostAsync(endpoint, content).Result;
            }

            return response;
        }

        protected BaseResponseModel<T> PostAPIRequest<T>(string endpoint, object model, bool addAuthorization = true)
        {
            var response = PostAPIRequest(endpoint, model, addAuthorization);

            if (!response.IsSuccessStatusCode)
            {
                // customize exception
                throw new Exception();
            }

            return ResolveResponse<T>(response.Content.ReadAsStringAsync().Result);
        }

        protected HttpResponseMessage GetAPIRequestAsync(string endpoint, object queryParams = null, bool addAuthorization = true)
        {
            HttpResponseMessage response = null;

            using (var client = CreateHttpClient(addAuthorization))
            {
                if (queryParams != null)
                {
                    if (!endpoint.EndsWith("/", StringComparison.InvariantCulture))
                    {
                        endpoint += "/";
                    }

                    endpoint += this.GetQueryString(queryParams);
                }

                response = client.GetAsync(endpoint).Result;
            }

            return response;
        }

        protected BaseResponseModel<T> GetAPIRequestAsync<T>(string endpoint, object queryParams = null, bool addAuthorization = true)
        {
            var response = GetAPIRequestAsync(endpoint, queryParams, addAuthorization);

            if (!response.IsSuccessStatusCode)
            {
                // customize exception
                throw new Exception();
            }

            return ResolveResponse<T>(response.Content.ReadAsStringAsync().Result);
        }

        private HttpClient CreateHttpClient(bool addAuthorization = true)
        {
            var client = new HttpClient() { BaseAddress = _baseUrl };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (addAuthorization)
            {
                var info = GetLoginInfo();

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {info.AccessToken}");
            }

            return client;
        }

        private string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return "?" + string.Join("&", properties.ToArray());
        }

        protected static TokenViewModel GetToken(string username, string password, string type)
        {
            TokenViewModel tokenViewModel = null;

            using (var client = new HttpClient() { BaseAddress = _baseUrl })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var content = new StringContent($"grant_type=password&username={username}&password={password}&app_type={type}", Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpResponseMessage httpResponseMessage = client.PostAsync(EndPoints.LOGIN, content).Result;

                string resultAsJSON = httpResponseMessage.Content.ReadAsStringAsync().Result;

                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK && resultAsJSON != null)
                {
                    tokenViewModel = JsonConvert.DeserializeObject<TokenViewModel>(resultAsJSON);
                }
            }

            return tokenViewModel;
        }

        protected virtual TokenViewModel SetLoginInfo(string info) => SetLoginInfo(JsonConvert.DeserializeObject<TokenViewModel>(info));

        protected virtual TokenViewModel SetLoginInfo(TokenViewModel tokenViewModel)
        {
            var currentSession = System.Web.HttpContext.Current.Session;

            if (tokenViewModel != null && tokenViewModel.IsExpired)
            {
                tokenViewModel = null;
                return tokenViewModel;
            }

            currentSession[SESSION_KEY] = JsonConvert.SerializeObject(tokenViewModel);

            return GetLoginInfo(false);
        }

        protected virtual TokenViewModel GetLoginInfo(bool clear = true)
        {
            var currentSession = System.Web.HttpContext.Current.Session;

            TokenViewModel tokenViewModel = currentSession[SESSION_KEY] == null ? null : JsonConvert.DeserializeObject<TokenViewModel>(currentSession[SESSION_KEY].ToString());

            if (clear && tokenViewModel != null && tokenViewModel.IsExpired)
            {
                ClearLoginInfo();
            }

            return tokenViewModel;
        }

        protected virtual void ClearLoginInfo()
        {
            var currentSession = System.Web.HttpContext.Current.Session;

            currentSession[SESSION_KEY] = null;
        }

        protected BaseResponseModel<T> ResolveResponse<T>(string data) => (BaseResponseModel<T>)JsonConvert.DeserializeObject<BaseResponseModel<T>>(data);

        protected AppUser CurrentUser { get => GetLoginInfo().UserData; }
    }
}
