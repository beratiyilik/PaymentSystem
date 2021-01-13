using Newtonsoft.Json;
using System;
namespace PaymentSystem.MVC.Hangfire.Helper
{
    public static class JsonHelper<T> where T : class
    {
        public static T JsonToModel(string jsonObject)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonObject);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
