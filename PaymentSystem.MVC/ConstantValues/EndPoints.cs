using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSystem.MVC.ConstantValues
{
    public static class EndPoints
    {
        public const string LOGIN = "/token";

        public const string MEMBERSHIP_REGISTER_MEMBERSHIP = "/api/Membership/RegisterMembership";
        public const string MEMBERSHIP_ENQUIRY_MEMBERSHIP = "/api/Membership/EnquiryMembership";
        public const string MEMBERSHIP_UNSUBCRIBE_MEMBERSHIP = "/api/Membership/UnsubcribeMembership";

        public const string PAYMENT_SYSTEM_GET_ACCRUAL_LOANS = "/api/PaymentSystem/GetAccrualLoans";
        public const string PAYMENT_SYSTEM_PAYMENT = "/api/PaymentSystem/Payment";
        public const string PAYMENT_SYSTEM_DEPOSIT_REFUND = "/api/PaymentSystem/DepositRefund";
    }
}
