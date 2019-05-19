using DAL.Interfaces;
using DAL.Abstracts;
using DAL.Repositories;
using DAL.Entities;
using System;

namespace DAL.RepositoryCreators
{
    public class EvaluationRepositoryCreator : IRepositoryCreator<Evaluation>
    {
        public Lazy<IGenericRepository<Evaluation>> GetRepositoryInstance(BaseContext context)
        {
            return new Lazy<IGenericRepository<Evaluation>>
                (() => new EvaluationRepository(context));
        }
    }
}
