namespace PaymentSystem.API.Models
{
    public class BaseResponseModel
    {
        public int ReturnCode { get; set; }

        public int? ErrorCode { get; set; }

        public string Message { get; set; }

        public object ResultData { get; set; }
    }
}