using System.Data.Entity;

namespace DAL
{
    public class AlbumDbInitializer
        : DropCreateDatabaseIfModelChanges<AlbumContext>
    {
        protected override void Seed(AlbumContext db)
        {
        }
    }
}
