using DAL.Abstracts;
using System;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.Entities;

namespace DAL.RepositoryCreators
{
    public class HashTagRepositoryCreator : IRepositoryCreator<HashTag>
    {
        public Lazy<IGenericRepository<HashTag>> GetRepositoryInstance(BaseContext context)
        {
            return new Lazy<IGenericRepository<HashTag>>
                (() => new HashTagRepository(context));
        }
    }
}
