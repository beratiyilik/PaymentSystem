using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Common.Entities.Base
{
    public interface IAudited<TKey> : ICreationAudited<TKey>, IModificationAudited<TKey> where TKey : struct
    {
    }
}
