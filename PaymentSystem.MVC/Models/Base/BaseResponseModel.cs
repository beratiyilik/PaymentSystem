using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSystem.MVC.Models.Base
{
    public class BaseResponseModel
    {
        public int ReturnCode { get; set; }

        public int? ErrorCode { get; set; }

        public string Message { get; set; }

        public object ResultData { get; set; }
    }

    public class BaseResponseModel<T> : BaseResponseModel
    {
        public new  T ResultData { get; set; }
    }

}