using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.Entities;
using System;

namespace DAL.RepositoryCreators
{
    public class ImageRepositoryCreator : IRepositoryCreator<Image>
    {
        public Lazy<IGenericRepository<Image>> GetRepositoryInstance(BaseContext context)
        {
            return new Lazy<IGenericRepository<Image>>
                (() => new ImageRepository(context));
        }
    }
}
