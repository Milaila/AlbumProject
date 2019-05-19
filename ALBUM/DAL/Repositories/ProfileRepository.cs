using DAL.Abstracts;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class ProfileRepository : GenericRepository<Profile>
    {
        private readonly BaseContext _context;

        public ProfileRepository(BaseContext context)
            : base(context)
            => _context = context;

        public override void Delete(int id)
        {
            Profile profile = Get(id)
                ?? throw new NotFoundException($"Profile with id = {id} was not found");
            _context.Set<Image>()
                .RemoveRange(profile.Images);
            _context.Set<Subscription>()
                .RemoveRange(profile.Subscribers);
            _context.Set<Subscription>()
                .RemoveRange(profile.Subscriptions);
            _context.Set<Evaluation>()
                .RemoveRange(profile.Evaluations);
            _entities.Remove(profile);
        }
    }
}
