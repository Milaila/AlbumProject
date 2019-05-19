using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EFUOW : IUnitOfWork
    {
        private CatalogContext _context;
        private IProductRepository _productRepository;
        private ISupplierRepository _supplierRepository;
        private ICategoryRepository _categoryRepository;

        public IProductRepository Products
        {
            get
            {
                if (_productRepository == null) 
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }
        public ISupplierRepository Suppliers
        {
            get
            {
                if (_supplierRepository == null)
                    _supplierRepository = new SupplierRepository(_context);
                return _supplierRepository;
            }
        }
        public ICategoryRepository Categories
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);
                return _categoryRepository;
            }
        }

        public EFUOW (CatalogContext context)
        {
            _context = context 
                ?? throw new NullReferenceException($"NULL {nameof(CatalogContext)}");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
