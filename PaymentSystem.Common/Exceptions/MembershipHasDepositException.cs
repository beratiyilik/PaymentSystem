using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Exceptions
{
    public class MembershipHasDepositException : Exception
    {
        private readonly string _message = string.Empty;

        public MembershipHasDepositException(string message) : base(message)
        {
            this._message = message;
        }

        internal MembershipHasDepositException(string message, Exception innerException) : base(message, innerException)
        {
            this._message = message;
        }

        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(this._message))
                    return this._message;
                return "Membership has avalable deposit!";
            }
        }
    }
}
