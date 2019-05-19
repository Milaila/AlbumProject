using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using BLL.DTO;
using BLL.Interfaces;
using AutoMapper;

namespace BLL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IAlbumUnitOfWork _db;
        private readonly IMapper _mapper;
        private bool _isDisposed = false;

        public ProfileService(IAlbumUnitOfWork db, MapperConfiguration mapperConfig)
        {
            _db = db
                ?? throw new ArgumentNullException("UnitOfWork must be not null!");
            _mapper = mapperConfig?.CreateMapper()
                ?? throw new ArgumentNullException("Mapper configuration must be not null!");
        }

        public int AddProfile(ProfileDTO profile)
        {
            ValidateProfile(profile);
            if (_db.ProfileRepository.GetAll().Any(x => x.UserId == profile.UserId))
                throw new ServiceException($"User (with id = {profile.UserId}) already has a profile!")
                {
                    Type = ExceptionType.UniqueException,
                    ExceptionValue = nameof(profile.UserId)
                };
            DAL.Entities.Profile profileDAL =_db.ProfileRepository
                .Add(_mapper.Map<DAL.Entities.Profile>(profile));
            _db.Save();
            return profileDAL.Id;
        }

        public void AddSubscription(int subscriptionProfileId, int profileId)
        {
            ValidateSubscription(profileId, subscriptionProfileId);
            _db.SubscriptionRepository.Add(new Subscription
            {
                SubscriberProfileId = profileId,
                SubscriptionProfileId = subscriptionProfileId
            });
            _db.Save();
        }

        public void PutSubscription(int subscriptionProfileId, int profileId)
        {
            if (_db.SubscriptionRepository
                    .GetAll()
                    .FirstOrDefault(x => (x.SubscriberProfileId == profileId) &&
                        (x.SubscriptionProfileId == subscriptionProfileId))
                    != null)
            AddSubscription(subscriptionProfileId, profileId);
        }

        public bool DeleteProfileById(int profileId)
        {
            if (!_db.ProfileRepository.Contains(profileId))
                return false;
            _db.ProfileRepository.Delete(profileId);
            _db.Save();
            return true;
        }

        public int UpdateOrCreateProfile(ProfileDTO profile)
        {
            if (profile == null)
                throw new ServiceException(ExceptionType.NullException, "Profile is null");
            var uniqueProfile = _db.ProfileRepository.GetAll()
                    .FirstOrDefault(x => x.UserId == profile.UserId);
            if (uniqueProfile != null && uniqueProfile.Id != profile.Id)
                throw new ServiceException(ExceptionType.UniqueException, 
                    $"User (with id = {profile.UserId}) already has a profile!");
            if (profile.Id < 1)
                return AddProfile(profile);
            DAL.Entities.Profile profileDAL = _db.ProfileRepository.Get(profile.Id);
            if (profileDAL == null)
                return AddProfile(profile);
            ValidateProfile(profile);
            profileDAL.BirthDate = profile.BirthDate;
            profileDAL.Description = profile.Description;
            profileDAL.Name = profile.Name;
            profileDAL.ImageFileId = profile.ImageFileId;
            _db.Save();
            return profile.Id;
        }

        public HashSet<ProfileDTO> GetAllProfiles()
        {
            return _mapper.Map<IQueryable<DAL.Entities.Profile>, HashSet<ProfileDTO>>
                    (_db.ProfileRepository.GetAll());
        }

        public ProfileDTO GetProfileById(int profileId)
        {
            return _mapper.Map<ProfileDTO>(_db.ProfileRepository.Get(profileId));
        }

        public HashSet<ProfileDTO> GetProfilesByName(string profileName)
        {
            if (profileName == null)
                throw new ServiceException(ExceptionType.NullException, "Profile name is empty");
            var profiles = _db.ProfileRepository.GetAll()
                .Where(x => x.Name.ToUpper().Contains(profileName.ToUpper()));
            return _mapper.Map<IEnumerable<DAL.Entities.Profile>, HashSet<ProfileDTO>>
                (profiles);
        }

        public HashSet<ProfileDTO> GetProfileSubscribers(int profileId)
        {
            DAL.Entities.Profile profile = _db.ProfileRepository.Get(profileId);
            if (profile == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            return _mapper.Map<IEnumerable<DAL.Entities.Profile>, HashSet<ProfileDTO>>
                    (profile.Subscribers.Select(x => x.SubscriberProfile));
        }

        public HashSet<ProfileDTO> GetProfileSubscriptions(int profileId)
        {
            DAL.Entities.Profile profile = _db.ProfileRepository.Get(profileId);
            if (profile == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            return GetProfileSubscriptionsByProfile(profile);
        }

        public bool RemoveProfileSubscriber(int subscriberProfileId, int profileId)
        {
            DAL.Entities.Profile profile = _db.ProfileRepository.Get(profileId);
            if (profile == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            int? subscriberId = profile.Subscribers
                .FirstOrDefault(x => x.SubscriberProfileId == subscriberProfileId)?
                .Id;
            if (subscriberId == null)
                return false;
            _db.SubscriptionRepository.Delete(subscriberId.Value);
            _db.Save();
            return true;
        }

        public bool RemoveProfileSubscription(int subscriptionProfileId, int profileId)
        {
            DAL.Entities.Profile profile = _db.ProfileRepository.Get(profileId);
            if (profile == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            int? subscriptionId = profile.Subscriptions
                .FirstOrDefault(x => x.SubscriptionProfileId == subscriptionProfileId)?
                .Id;
            if (subscriptionId == null)
                return false;
            _db.SubscriptionRepository.Delete(subscriptionId.Value);
            _db.Save();
            return true;
        }

        void IDisposable.Dispose()
        {
            if (!_isDisposed)
            {
                (_db as IDisposable)?.Dispose();
                _isDisposed = true;
            }
            GC.SuppressFinalize(this);
        }

        private void ValidateSubscription(int profileId, int subscriptionProfileId)
        {
            if (subscriptionProfileId == profileId)
                throw new ServiceException("Profile can not subscribe to itself.")
                {
                    Type = ExceptionType.SameException,
                };
            DAL.Entities.Profile profile = _db.ProfileRepository.Get(profileId);
            if (profile == null)
                throw new ServiceException($"Profile with id = {profileId} was not found!")
                {
                    Type = ExceptionType.ForeignKeyException
                };
            if (!_db.ProfileRepository.Contains(subscriptionProfileId))
                throw new ServiceException($"Profile with id = {subscriptionProfileId} was not found!")
                {
                    Type = ExceptionType.ForeignKeyException
                };
            if (GetProfileSubscriptionsByProfile(profile)
                    .Select(x => x.Id)
                    .Contains(subscriptionProfileId))
                throw new ServiceException(ExceptionType.UniqueException, 
                    $"Profile already has a subscription with id = {subscriptionProfileId}");
        }

        private HashSet<ProfileDTO> GetProfileSubscriptionsByProfile(DAL.Entities.Profile profile)
        {
            return _mapper.Map<IEnumerable<DAL.Entities.Profile>, HashSet<ProfileDTO>>
                (
                    profile?
                    .Subscriptions
                    .Select(x => x.SubscriptionProfile)
                );
        }

        private void ValidateProfile(ProfileDTO profile)
        {
            if (profile == null)
                throw new ServiceException(ExceptionType.NullException, "Profile is null!");
            if ((profile.Name == null) || (profile.Name.Trim() == ""))
                throw new ServiceException("Profile name is null!")
                {
                    Type = ExceptionType.EmptyStringException
                };
            if (profile.UserId == null)
                throw new ServiceException("User id is empty!")
                {
                    Type = ExceptionType.ForeignKeyException,
                    ExceptionValue = profile.UserId.ToString()
                };
            if ((profile.ImageFileId != null) && !_db.FileRepository.Contains(profile.ImageFileId.Value))
                throw new ServiceException($"Image file with id = { profile.ImageFileId } was not found!")
                {
                    Type = ExceptionType.ForeignKeyException,
                    ExceptionValue = profile.ImageFileId.ToString()
                };
            if (profile.BirthDate != null && profile.BirthDate < new DateTime(1753, 1, 1))
                throw new ServiceException()
                {
                    Type = ExceptionType.InvalidDate,
                    ExceptionValue = profile.BirthDate.ToString()
                };
        }

        public ProfileDTO GetProfileByUser(string userId)
        {
            return _mapper.Map<ProfileDTO>(_db.ProfileRepository.GetAll()
                .FirstOrDefault(x => x.UserId == userId));
        }
    }
}
