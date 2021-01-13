using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PaymentSystem.Common.Enum.Enum;

namespace PaymentSystem.Application.Models.Base
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public EntityState State { get; set; }
    }
}
