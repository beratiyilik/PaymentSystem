using Newtonsoft.Json;

namespace PaymentSystem.MVC.Hangfire.Model
{
    public class HFLoginModel
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
    }

    public class HFLoginRequestModel
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("app_type")]
        public string AppType { get; set; }
    }
}
