using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Configurations
{
    public class ImageConfiguration : EntityTypeConfiguration<Image>
    {
        public ImageConfiguration()
        {
            Property(x => x.Time)
                .IsRequired();
            HasMany(x => x.Rating)
                .WithRequired(ev => ev.Image)
                .WillCascadeOnDelete(true);
            HasOptional(x => x.ImageFile)
                .WithMany()
                .HasForeignKey(x => x.ImageFileId);
        }
    }
}
