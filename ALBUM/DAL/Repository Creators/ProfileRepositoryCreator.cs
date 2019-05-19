using DAL.Interfaces;
using DAL.Repositories;
using DAL.Entities;
using DAL.Abstracts;
using System;

namespace DAL.RepositoryCreators
{
    public class ProfileRepositoryCreator : IRepositoryCreator<Profile>
    {
        public Lazy<IGenericRepository<Profile>> GetRepositoryInstance(BaseContext context)
        {
            return new Lazy<IGenericRepository<Profile>>
                (() => new ProfileRepository(context));
        }
    }
}
