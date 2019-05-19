using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class FileRepository : GenericRepository<File>
    {
        public FileRepository(BaseContext context)
            : base(context)
        { }
    }
}
