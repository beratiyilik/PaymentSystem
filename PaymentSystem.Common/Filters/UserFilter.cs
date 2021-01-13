using System;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Common.Filters
{
    public class UserFilter /* : UserModel */
    {
        public Guid Id { get; set; }

        public EntityType EntityType { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public string TaxNumber { get; set; }
    }
}
