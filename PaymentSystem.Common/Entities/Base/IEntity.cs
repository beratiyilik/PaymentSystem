using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentSystem.Common.Entities.Base
{
    public interface IEntity<TKey> : IAudited<TKey>, IHasState, IHasTimestamp where TKey : struct
    {
        TKey Id { get; /* set; */ }
    }
}
