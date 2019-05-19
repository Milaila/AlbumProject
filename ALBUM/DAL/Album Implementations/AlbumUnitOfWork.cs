using DAL.Abstracts;
using DAL.Entities;
using DAL.Interfaces;
using System;

namespace DAL
{
    public class AlbumUnitOfWork : IAlbumUnitOfWork
    {
        private readonly BaseContext _context;
        private bool _isDisposed = false;

        private readonly Lazy<IGenericRepository<Profile>> _profileRepository;
        private readonly Lazy<IGenericRepository<File>> _fileRepository;
        private readonly Lazy<IGenericRepository<Subscription>> _subscriptionRepository;
        private readonly Lazy<IGenericRepository<Evaluation>> _evaluationRepository;
        private readonly Lazy<IGenericRepository<HashTag>> _hashTagRepository;
        private readonly Lazy<IGenericRepository<Image>> _imageRepository;

        public IGenericRepository<Profile> ProfileRepository
            => _profileRepository.Value;
        public IGenericRepository<File> FileRepository
            => _fileRepository.Value;
        public IGenericRepository<Subscription> SubscriptionRepository
            => _subscriptionRepository.Value;
        public IGenericRepository<Evaluation> EvaluationRepository
            => _evaluationRepository.Value;
        public IGenericRepository<HashTag> HashTagRepository
            => _hashTagRepository.Value;
        public IGenericRepository<Image> ImageRepository
            => _imageRepository.Value;

        public AlbumUnitOfWork
            (
                BaseContext context,
                IRepositoryCreator<Profile> profileRepositoryCreator,
                IRepositoryCreator<Image> imageRepositoryCreator,
                IRepositoryCreator<HashTag> hashTagRepositoryCreator,
                IRepositoryCreator<Evaluation> evaluationRepositoryCreator,
                IRepositoryCreator<Subscription> subscriptionRepositoryCreator,
                IRepositoryCreator<File> fileRepositoryCreator
            )
        {
            _context = context
                ?? throw new ArgumentNullException("Context must be not null!");
            _hashTagRepository = hashTagRepositoryCreator.GetRepositoryInstance(context)
                ?? throw new ArgumentNullException("RepositoryCreator must be not null!");
            _profileRepository = profileRepositoryCreator.GetRepositoryInstance(context)
                ?? throw new ArgumentNullException("RepositoryCreator must be not null!");
            _fileRepository = fileRepositoryCreator.GetRepositoryInstance(context)
                ?? throw new ArgumentNullException("RepositoryCreator must be not null!");
            _imageRepository = imageRepositoryCreator.GetRepositoryInstance(context)
                ?? throw new ArgumentNullException("RepositoryCreator must be not null!");
            _subscriptionRepository = subscriptionRepositoryCreator.GetRepositoryInstance(context)
                ?? throw new ArgumentNullException("RepositoryCreator must be not null!");
            _evaluationRepository = evaluationRepositoryCreator.GetRepositoryInstance(context)
                ?? throw new ArgumentNullException("RepositoryCreator must be not null!");
        }

        void IDisposable.Dispose()
        {
            if (!_isDisposed)
            {
                _context.Dispose();
                _isDisposed = true;
            }
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
