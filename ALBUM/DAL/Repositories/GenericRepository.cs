using System;
using System.Data.Entity;
using DAL.Abstracts;
using System.Linq;

namespace DAL.Interfaces
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
        where TEntity : Entity
    {
        protected DbSet<TEntity> _entities;

        public GenericRepository(BaseContext context)
        {
            _entities = context?.Set<TEntity>()
                ?? throw new ArgumentException($"Context must be not null!");
        }

        public virtual TEntity Add(TEntity item)
            => _entities.Add(item
                ?? throw new ArgumentNullException("Entity must be not null!"));

        public virtual bool Contains(int id)
            => _entities.Any(x => x.Id == id);

        public virtual void Delete(int id)
            => _entities.Remove(Get(id)
                ?? throw new NotFoundException($"Entity with id = {id} was not found"));

        public virtual TEntity Get(int id)
            => _entities.Find(id);

        public virtual IQueryable<TEntity> GetAll()
            => _entities;
    }
}
