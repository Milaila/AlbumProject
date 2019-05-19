using DAL.Abstracts;
using System.Linq;

namespace DAL.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : Entity
    {
        TEntity Get(int id);
        TEntity Add(TEntity entity);
        void Delete(int id);
        IQueryable<TEntity> GetAll();
        bool Contains(int id);
    }
}
