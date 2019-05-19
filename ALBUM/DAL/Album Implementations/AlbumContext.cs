using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using DAL.Entities;
using DAL.Abstracts;
using Ninject;

namespace DAL
{
    public class AlbumContext: BaseContext
    {
        [Inject]
        public EntityTypeConfiguration<Profile> ProfileConfig { get; set; }
        [Inject]
        public EntityTypeConfiguration<Image> ImageConfig { get; set; }
        [Inject]
        public EntityTypeConfiguration<File> FileConfig { get; set; }
        [Inject]
        public EntityTypeConfiguration<HashTag> HashTagConfig { get; set; }
        [Inject]
        public EntityTypeConfiguration<Evaluation> EvaluationConfig { get; set; }
        [Inject]
        public EntityTypeConfiguration<Subscription> SubscriptionConfig { get; set; }

        public AlbumContext
            (string connectionString, IDatabaseInitializer<AlbumContext> dbInitializer)
            : base(connectionString)
        {
            if (dbInitializer != null)
                Database.SetInitializer(dbInitializer);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (HashTagConfig != null)
                modelBuilder.Configurations.Add(HashTagConfig);
            if (ProfileConfig != null)
                modelBuilder.Configurations.Add(ProfileConfig);
            if (ImageConfig != null)
                modelBuilder.Configurations.Add(ImageConfig);
            if (FileConfig != null)
                modelBuilder.Configurations.Add(FileConfig);
            if (EvaluationConfig != null)
                modelBuilder.Configurations.Add(EvaluationConfig);
            if (SubscriptionConfig != null)
                modelBuilder.Configurations.Add(SubscriptionConfig);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
