using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class HashTagRepository : GenericRepository<HashTag>
    {
        public HashTagRepository(BaseContext context)
            : base(context)
        { }
    }
}
