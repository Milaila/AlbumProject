using System.Data.Entity;

namespace DAL.Abstracts
{
    public abstract class BaseContext : DbContext
    {
        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity
            => base.Set<TEntity>();

        public BaseContext(string connectionString)
            : base(connectionString)
        { }
    }
}
