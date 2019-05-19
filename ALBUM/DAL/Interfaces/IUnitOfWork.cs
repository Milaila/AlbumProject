using System;
using DAL.Abstracts;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : Entity;
    }
}
