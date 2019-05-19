using System;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IAlbumUnitOfWork : IDisposable
    {
        IGenericRepository<Profile> ProfileRepository { get; }
        IGenericRepository<File> FileRepository { get; }
        IGenericRepository<Subscription> SubscriptionRepository { get; }
        IGenericRepository<Evaluation> EvaluationRepository { get; }
        IGenericRepository<HashTag> HashTagRepository { get; }
        IGenericRepository<Image> ImageRepository { get; }
        void Save();
    }
}
