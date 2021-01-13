using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Common.Enum
{
    public class Enum
    {
        [Flags]
        // [BindableTypeAttribute(IsBindable = false)]
        [Bindable(BindableSupport.No)]
        [Description("Entity State")]
        public enum EntityState
        {
            [Description("Active")]
            Active = 1,
            [Description("Passive")]
            Passive = 2,
            [Description("Deleted")]
            Deleted = 3
        }

        [Description("Operation Result")]
        public enum OperationResult
        {
            [Display(Description = "Initial Value", Name = "None")]
            [Description("None")]
            Null = 0,
            [Display(Description = "The operation completed successfully", Name = "OK")]
            [Description("OK")]
            OK = 1,
            [Display(Description = "The Operation Failed!", Name = "Error")]
            [Description("Error")]
            Err = 2,
            [Display(Description = "The operation completed successfully, but no data returned!", Name = "No Data")]
            [Description("No Data")]
            NoData = 3
        }

        public enum ErrorCode
        {
            NoneError,
            ValidError,
            UnknownError,
            ItemNotFound,
            WrongFormatGsmEmail
        }

        public enum Active
        {
            False,
            True,
            TrueOrFalse
        }

        [Description("User Type")]
        public enum UserType
        {
            [Description("Membership")]
            Membership = 0,
            [Description("Manager")]
            Manager = 1
        }

        [Description("Entity Type")]
        public enum EntityType
        {
            [Description("Natural Person")]
            NaturalPerson = 0,
            [Description("Legal Entity")]
            LegalEntity = 1
        }

        public enum AccrualLoanType
        {
            [Description("Invoice")]
            Invoice = 0,
            [Description("Deposit")]
            Deposit = 1
        }
    }
}
