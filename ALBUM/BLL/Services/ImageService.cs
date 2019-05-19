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
    public class ImageService : IImageService
    {
        private readonly IAlbumUnitOfWork _db;
        private readonly IMapper _mapper;
        private bool _isDisposed = false;

        public ImageService(IAlbumUnitOfWork db, MapperConfiguration mapperConfig)
        {
            _db = db
                ?? throw new ArgumentNullException("UnitOfWork must be not null!");
            _mapper = mapperConfig?.CreateMapper()
                ?? throw new ArgumentNullException("Mapper configuration must be not null!");
        }

        public void PutHashTagsToImage(int imageId, HashSet<int> hashTags)
        {
            if (hashTags == null)
                throw new ServiceException(ExceptionType.NullException, "Hash tags are null");
            Image image = _db.ImageRepository.Get(imageId);
            if (image == null)
                throw new ServiceException(ExceptionType.NotFoundException, 
                    $"Image with id = {imageId} was not found!");
            PutHashTagsToImage(image, hashTags);
        }

        public void AddHashTagToImage(int imageId, int hashTagId)
        {
            Image image = _db.ImageRepository.Get(imageId);
            if (image == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Image with id = {imageId} was not found!");
            HashTag hashTag = _db.HashTagRepository.Get(hashTagId);
            if (hashTag == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Tag with id = {hashTagId} was not found!");
            image.HashTags.Add(hashTag);
            _db.Save();
        }

        public ImageDTO GetImageById(int imageId)
        {
            return _mapper.Map<ImageDTO>(_db.ImageRepository.Get(imageId));
        }

        public int AddImage(ImageDTO image, HashSet<int> hashTags = null)
        {
            ValidateImage(image);
            Image imageDAL = _db.ImageRepository.Add(_mapper.Map<Image>(image));
            _db.Save();
            PutHashTagsToImage(imageDAL, hashTags);
            return imageDAL.Id;
        }

        public bool DeleteImageById(int imageId)
        {
            if (!_db.ImageRepository.Contains(imageId))
                return false;
            _db.ImageRepository.Delete(imageId);
            _db.Save();
            return true;
        }

        public int UpdateOrCreateImage(ImageDTO image, HashSet<int> hashTags = null)
        {
            if (image == null)
                throw new ServiceException(ExceptionType.NullException, "Image to update is null");
            if (image.Id < 1)
                return AddImage(image, hashTags);
            Image imageDAL = _db.ImageRepository.Get(image.Id);
            if (imageDAL == null)
                return AddImage(image, hashTags);
            ValidateImage(image);
            PutHashTagsToImage(imageDAL, hashTags);
            imageDAL.ImageFileId = image.ImageFileId;
            imageDAL.Time = image.Time ?? DateTime.UtcNow;
            imageDAL.Title = image.Title;
            _db.Save();
            return imageDAL.Id;
        }

        public HashSet<ImageDTO> GetImagesByHashTagAndProfile(int hashTagId, int profileId)
        {
            HashTag hashTag = _db.HashTagRepository.Get(hashTagId);
            if (hashTag == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Tag with id = {hashTagId} was not found!");
            if (!_db.ProfileRepository.Contains(profileId))
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            var images = _db.ImageRepository.GetAll()
                .Where(x =>
                    (x.ProfileId == profileId) && x.HashTags.Contains(hashTag));
            return _mapper.Map<IQueryable<Image>, HashSet<ImageDTO>>(images);
        }

        public HashSet<ImageDTO> GetImagesByHashTag(int hashTagId)
        {
            HashTag hashTag = _db.HashTagRepository.Get(hashTagId);
            if (hashTag == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Tag with id = {hashTagId} was not found!");
            var images = _db.ImageRepository.GetAll()
                .Where(x => x.HashTags.Contains(hashTag));
            return _mapper.Map<IQueryable<Image>, HashSet<ImageDTO>>(images);
        }

        public HashSet<ImageDTO> GetImagesByProfile(int profileId)
        {
            if (!_db.ProfileRepository.Contains(profileId))
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            var images = _db.ImageRepository.GetAll()
                .Where(x => x.ProfileId == profileId);
            return _mapper.Map<IQueryable<Image>, HashSet<ImageDTO>>(images);
        }

        public HashSet<ImageDTO> GetImagesByTitleAndProfile (string subTitle, int profileId)
        {
            if (!(subTitle?.Trim().Length > 0))
                throw new ServiceException(ExceptionType.EmptyStringException, 
                    "Title search is empty!");
            if (!_db.ProfileRepository.Contains(profileId))
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            var images = _db.ImageRepository.GetAll()
                .Where(x =>
                    (x.ProfileId == profileId) && 
                    x.Title.ToUpper().Contains(subTitle.ToUpper()));
            return _mapper.Map<IQueryable<Image>, HashSet<ImageDTO>>(images);
        }

        public HashSet<ImageDTO> GetImagesByTitle(string subTitle)
        {
            if (!(subTitle?.Trim().Length > 0))
                throw new ServiceException(ExceptionType.EmptyStringException,
                    "Title search is empty!");
            var images = _db.ImageRepository.GetAll()
                .Where(x => x.Title.ToUpper().Contains(subTitle.ToUpper()));
            return _mapper.Map<IQueryable<Image>, HashSet<ImageDTO>>(images);
        }

        public HashSet<ImageDTO> GetAllImages()
        {
            return _mapper.Map<IEnumerable<Image>, HashSet<ImageDTO>>
                    (_db.ImageRepository.GetAll().ToList());
        }

        public double GetAverageMarkByImage(int imageId)
        {
            Image image = _db.ImageRepository.Get(imageId);
            if (image == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Image with id = {imageId} was not found!");
            return image.Rating.Count == 0 ? 0 :
                image.Rating.Average(x => x.Mark);
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

        private void CheckHashTags(ICollection<int> hashTags)
        {
            if (hashTags == null)
                throw new ServiceException(ExceptionType.NullException, 
                    "Tags are null");
            foreach (var tag in hashTags)
            {
                if (!_db.HashTagRepository.Contains(tag))
                    throw new ServiceException(ExceptionType.NotFoundException,
                        $"Tag with id = {tag} was not found!");
            }
        }

        private void ValidateImage(ImageDTO image)
        {
            if (image == null)
                throw new ServiceException(ExceptionType.NullException, "Image is null!");
            if (image.Time == null)
                image.Time = DateTime.UtcNow;
            if (image.Time < new DateTime(1753, 1, 1))
                throw new ServiceException("Invalid time!")
                {
                    Type = ExceptionType.InvalidDate,
                    ExceptionValue = image.Time.ToString()
                };
            if (!_db.ProfileRepository.Contains(image.ProfileId))
                throw new ServiceException($"Profile with id = {image.ProfileId} was not found!")
                {
                    Type = ExceptionType.ForeignKeyException,
                };
            if (image.ImageFileId != null && !_db.FileRepository.Contains(image.ImageFileId.Value))
                throw new ServiceException($"Image file with id = {image.ImageFileId} was not found")
                {
                    Type = ExceptionType.ForeignKeyException,
                };
        }

        private void PutHashTagsToImage(Image image, HashSet<int> hashTags)
        {
            if (image == null)
                throw new ServiceException(ExceptionType.NullException, "Image is null");
            if (hashTags == null)
            {
                image.HashTags.Clear();
                return;
            }
            CheckHashTags(hashTags);
            foreach (var tag in hashTags)
                image.HashTags.Add(_db.HashTagRepository.Get(tag));
        }
    }
}
