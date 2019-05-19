using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class ImageRepository : GenericRepository<Image>
    {
        public ImageRepository(BaseContext context)
            : base(context)
        { }
    }
}
