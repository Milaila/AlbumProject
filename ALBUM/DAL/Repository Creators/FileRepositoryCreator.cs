using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.Entities;
using System;

namespace DAL.RepositoryCreators
{
    public class FileRepositoryCreator : IRepositoryCreator<File>
    {
        public Lazy<IGenericRepository<File>> GetRepositoryInstance
            (BaseContext context)
        {
            return new Lazy<IGenericRepository<File>>
                (() => new FileRepository(context));
        }
    }
}
