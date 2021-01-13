using Newtonsoft.Json;
using PaymentSystem.MVC.Hangfire.ConstantValues;
using PaymentSystem.MVC.Hangfire.Helper;
using PaymentSystem.MVC.Hangfire.Model;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.MVC.Hangfire.Job
{
    public class ApiCallServiceJob : IDisposable
    {
        private static string _baseUrlAsString = "https://localhost:44399";

        private static Uri _baseUrl = new Uri(_baseUrlAsString);

        public ApiCallServiceJob()
        {

        }

        public void DoWork()
        {
            this.TriggerMonthlyBilling();
        }

        private void TriggerMonthlyBilling()
        {
            this.GetAsync(EndPoints.HANGFIRE_TRIGGER_MONTHLY_BILLING);
        }

        private HttpClient PrepareHttpClientForRequest()
        {
            var client = new HttpClient() { BaseAddress = _baseUrl };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.GetToken()}");
            return client;
        }

        private string GetToken()
        {
            var token = string.Empty;
            try
            {
                using (var client = new HttpClient() { BaseAddress = _baseUrl })
                {
                    var requestModel = new HFLoginRequestModel
                    {
                        UserName = ConfigurationManager.AppSettings["username"].ToString(),
                        Password = ConfigurationManager.AppSettings["password"].ToString(),
                        GrantType = "password",
                        AppType = ConfigurationManager.AppSettings["apptype"].ToString()
                    };

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var content = new StringContent($"grant_type=password&username={requestModel.UserName}&password={requestModel.Password}&app_type=1", Encoding.UTF8, "application/x-www-form-urlencoded");

                    var response = client.PostAsync(EndPoints.LOGIN, content).Result;

                    var responseString = response.Content.ReadAsStringAsync().Result;

                    var result = JsonHelper<HFLoginModel>.JsonToModel(responseString);

                    token = $"{result.Token}";
                }

            }
            catch (Exception)
            {
                throw;
            }
            return token;
        }

        private void GetAsync(string apiEndPoint)
        {
            try
            {
                using (var httpClient = this.PrepareHttpClientForRequest())
                {
                    var response = httpClient.GetAsync(apiEndPoint).Result;

                    var result = response.Content;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string PostAsync<T>(T model, string apiEndPoint)
        {
            var result = string.Empty;
            try
            {
                using (var httpClient = this.PrepareHttpClientForRequest())
                {
                    var objectAsJSON = JsonConvert.SerializeObject(model);

                    var content = new StringContent(objectAsJSON, Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(apiEndPoint, content).Result;

                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public void Dispose()
        {

        }
    }
}
