using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class EvaluationRepository : GenericRepository<Evaluation>
    {
        public EvaluationRepository(BaseContext context)
            : base(context)
        { }
    }
}
