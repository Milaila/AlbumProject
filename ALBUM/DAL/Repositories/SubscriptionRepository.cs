using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription>
    {
        public SubscriptionRepository(BaseContext context)
            : base(context)
        { }
    }
}
