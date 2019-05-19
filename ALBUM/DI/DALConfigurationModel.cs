using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using DAL.Entities;
using Ninject.Modules;
using DAL.Interfaces;
using DAL.Abstracts;
using DAL.RepositoryCreators;
using DAL.Repositories;
using DAL.Configurations;
using DAL;

namespace DI
{
    public class DALConfigurationModel : NinjectModule
    {
        private readonly string _connectionString;

        public DALConfigurationModel(string connectionString)
            => _connectionString = connectionString;

        public override void Load()
        {
            Bind<EntityTypeConfiguration<Image>>()
                .To<ImageConfiguration>();
            Bind<EntityTypeConfiguration<Evaluation>>()
                .To<EvaluationConfiguration>();
            Bind<EntityTypeConfiguration<HashTag>>()
                .To<HashTagConfiguration>();
            Bind<EntityTypeConfiguration<Profile>>()
                .To<ProfileConfiguration>();
            Bind<EntityTypeConfiguration<File>>()
                .To<FileConfiguration>();
            Bind<EntityTypeConfiguration<Subscription>>()
                .To<SubscriptionConfiguration>();
            Bind<IGenericRepository<HashTag>>()
                .To<HashTagRepository>();
            Bind<IRepositoryCreator<HashTag>>()
                .To<HashTagRepositoryCreator>();
            Bind<IGenericRepository<File>>()
                .To<FileRepository>();
            Bind<IGenericRepository<Evaluation>>()
                .To<EvaluationRepository>();
            Bind<IRepositoryCreator<Evaluation>>()
                .To<EvaluationRepositoryCreator>();
            Bind<IGenericRepository<Profile>>()
                .To<ProfileRepository>();
            Bind<IRepositoryCreator<Profile>>()
                .To<ProfileRepositoryCreator>();
            Bind<IGenericRepository<Image>>()
                .To<ImageRepository>();
            Bind<IRepositoryCreator<File>>()
                .To<FileRepositoryCreator>();
            Bind<IRepositoryCreator<Image>>()
                .To<ImageRepositoryCreator>();
            Bind<IGenericRepository<Subscription>>()
                .To<SubscriptionRepository>();
            Bind<IRepositoryCreator<Subscription>>()
                .To<SubscriptionRepositoryCreator>();
            Bind<IDatabaseInitializer<AlbumContext>>()
                .To<AlbumDbInitializer>();
            Bind<IUnitOfWork>()
                .To<UnitOfWork>();
            Bind<IAlbumUnitOfWork>()
                .To<AlbumUnitOfWork>();
            Bind<BaseContext>()
                .To<AlbumContext>()
                .InSingletonScope()
                .WithConstructorArgument(_connectionString);
        }
    }
}
