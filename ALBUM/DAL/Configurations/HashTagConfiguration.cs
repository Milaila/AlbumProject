using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Configurations
{
    public class HashTagConfiguration : EntityTypeConfiguration<HashTag>
    {
        public HashTagConfiguration()
        {
            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
