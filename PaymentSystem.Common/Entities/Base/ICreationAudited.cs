using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Entities.Base
{
    public interface ICreationAudited<TKey> : IHasCreationTime where TKey : struct
    {
        TKey CreatedById { get; set; }
    }
}
