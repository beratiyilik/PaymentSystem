using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.API.Filters
{
    public class MembershipFilter
    {
        public EntityType EntityType { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public string TaxNumber { get; set; }
    }
}