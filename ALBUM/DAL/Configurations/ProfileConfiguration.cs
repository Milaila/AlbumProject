using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Configurations
{
    public class ProfileConfiguration : EntityTypeConfiguration<Profile>
    {
        public ProfileConfiguration()
        {
            HasMany(x => x.Subscribers)
                .WithRequired(s => s.SubscriptionProfile);
            HasMany(x => x.Subscriptions)
                .WithRequired(s => s.SubscriberProfile);
            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);
            Property(x => x.Description)
                .HasMaxLength(1000);
            HasOptional(x => x.ImageFile)
                .WithMany()
                .HasForeignKey(x => x.ImageFileId);
        }
    }
}
