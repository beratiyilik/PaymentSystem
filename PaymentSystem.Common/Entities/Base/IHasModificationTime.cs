using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Entities.Base
{
    public interface IHasModificationTime
    {
        DateTime? LastModifiedAt { get; set; }
    }
}
