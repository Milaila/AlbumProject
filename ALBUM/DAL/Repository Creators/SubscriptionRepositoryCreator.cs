using DAL.Interfaces;
using DAL.Repositories;
using DAL.Abstracts;
using System;
using DAL.Entities;

namespace DAL.RepositoryCreators
{
    public class SubscriptionRepositoryCreator : IRepositoryCreator<Subscription>
    {
        public Lazy<IGenericRepository<Subscription>> GetRepositoryInstance(BaseContext context)
        {
            return new Lazy<IGenericRepository<Subscription>>
                (() => new SubscriptionRepository(context));
        }
    }
}
