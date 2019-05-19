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
    public class HashTagService : IHashTagService
    {
        private readonly IAlbumUnitOfWork _db;
        private readonly IMapper _mapper;
        private bool _isDisposed = false;

        public HashTagService(IAlbumUnitOfWork db, MapperConfiguration mapperConfig)
        {
            _db = db
                ?? throw new ArgumentNullException("UnitOfWork must be not null!");
            _mapper = mapperConfig?.CreateMapper()
                ?? throw new ArgumentNullException("Mapper configuration must be not null!");
        }

        public int AddHashTag(HashTagDTO hashTag)
        {
            ValidateHashTag(hashTag);
            if (GetHashTagByName(hashTag.Name) != null)
                throw new ServiceException(ExceptionType.UniqueException, "Hash tag already exists!");
            HashTag hashTagDAL = _db.HashTagRepository.Add(_mapper.Map<HashTag>(hashTag));
            _db.Save();
            return hashTagDAL.Id;
        }

        public int PutHashTag(HashTagDTO hashTag)
        {
            if (hashTag == null)
                throw new ServiceException(ExceptionType.NullException, "Hash tag is null");
            int? hashTagId = _db.HashTagRepository.Get(hashTag.Id)?.Id
                ?? GetHashTagByName(hashTag.Name)?.Id;
            return hashTagId ?? AddHashTag(hashTag);
        }

        public HashSet<int> PutHashTags(HashSet<HashTagDTO> hashTags)
        {
            if (hashTags == null)
                throw new ServiceException(ExceptionType.NullException, "Hash tags are null");
            HashSet<int> tags = new HashSet<int>();
            foreach (var tag in hashTags)
            {
                tags.Add(PutHashTag(tag));
            }
            _db.Save();
            return new HashSet<int>(tags);
        }

        public HashSet<int> PutHashTagsByName(HashSet<string> hashTags)
        {
            if (hashTags == null)
                throw new ServiceException(ExceptionType.NullException, "Hash tags are null");
            HashSet<int> tags = new HashSet<int>();
            foreach (var tag in hashTags)
            {
                tags.Add(PutHashTag(new HashTagDTO
                {
                    Name = tag
                }));
            }
            return tags;
        }

        public bool DeleteHashTagById(int hashTagId)
        {
            if (!_db.HashTagRepository.Contains(hashTagId))
                return false;
            _db.HashTagRepository.Delete(hashTagId);
            _db.Save();
            return true;
        }

        public HashTagDTO GetHashTagById(int hashTagId)
        {
            return _mapper.Map<HashTagDTO>(_db.HashTagRepository.Get(hashTagId));
        }

        public HashSet<HashTagDTO> GetHashTagsByImage(int imageId)
        {
            Image image = _db.ImageRepository.Get(imageId);
            if (image == null)
                throw new ServiceException(ExceptionType.NotFoundException, 
                    $"Image with id = {imageId} was not found!");
            return _mapper.Map<ICollection<HashTag>, HashSet<HashTagDTO>>(image.HashTags);
        }

        public HashSet<HashTagDTO> GetHashTagsByPartOfName(string name)
        {
            return _mapper.Map<IQueryable<HashTag>, HashSet<HashTagDTO>>(_db.HashTagRepository
                    .GetAll()
                    .Where(x => x.Name.ToUpper().Contains(name.ToUpper())));
        }

        public HashTagDTO GetHashTagByName(string hashTagName)
        {
            return _mapper.Map<HashTagDTO>(GetDALHashTagByName(hashTagName));
        }

        public HashSet<HashTagDTO> GetAllHashTags()
        {
            return _mapper.Map<IEnumerable<HashTag>, HashSet<HashTagDTO>>
                    (_db.HashTagRepository.GetAll().ToList());
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

        private HashTag GetDALHashTagByName(string hashTagName)
        {
            if (hashTagName == null)
                return null;
            return _db.HashTagRepository
                    .GetAll()
                    .FirstOrDefault(x =>
                        x.Name.Trim().ToUpper() == hashTagName.Trim().ToUpper());
        }

        private void ValidateHashTag(HashTagDTO hashTag)
        {
            if (hashTag == null)
                throw new ServiceException("Hash tag is null!")
                {
                    Type = ExceptionType.NullException
                };
            if (!(hashTag.Name?.Trim().Length > 0))
                throw new ServiceException("Hash tag name is empty")
                {
                    Type = ExceptionType.EmptyStringException,
                    ExceptionValue = nameof(hashTag.Name)
                };
        }
    }
}
