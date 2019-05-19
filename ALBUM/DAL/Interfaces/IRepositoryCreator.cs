using DAL.Abstracts;

namespace DAL.Interfaces
{
    public interface IRepositoryCreator<TEntity> 
        where TEntity : Entity
    {
        System.Lazy<IGenericRepository<TEntity>> 
            GetRepositoryInstance(BaseContext context);
    }
}
