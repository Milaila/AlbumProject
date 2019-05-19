using System;
using DAL.Abstracts;
using DAL.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseContext _context;
        private bool _isDisposed = false;

        public UnitOfWork (BaseContext context)
        {
            _context = context
                ?? throw new ArgumentNullException($"Context must be not null!");
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _context.Dispose();
                _isDisposed = true;
            }
            GC.SuppressFinalize(this);
        }

        public void Save()
            => _context.SaveChanges();

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
            => new GenericRepository<TEntity>(_context);
    }
}
