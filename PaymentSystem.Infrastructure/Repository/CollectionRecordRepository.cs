using PaymentSystem.Common.Entities;
using PaymentSystem.Common.Repositories;
using PaymentSystem.Common.Data;
using PaymentSystem.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Infrastructure.Repository
{
    public class CollectionRecordRepository : Repository<CollectionRecord>, ICollectionRecordRepository
    {
        public CollectionRecordRepository(PaymentSystemContext dbContext) : base(dbContext)
        {

        }
    }
}
