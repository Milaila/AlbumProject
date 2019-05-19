using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Configurations
{
    public class FileConfiguration : EntityTypeConfiguration<File>
    {
        public FileConfiguration()
        {
            Property(x => x.Name)
                .IsRequired();
            Property(x => x.Folder)
                .IsRequired();
        }
    }
}
