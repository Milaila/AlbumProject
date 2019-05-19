using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using BLL.DTO;
using BLL.Interfaces;
using AutoMapper;
using System.Text;

namespace BLL.Services
{
    public class ImageFileService : IImageFileService
    {
        private readonly IAlbumUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly Lazy<List<string>> _imageExtensions;
        private readonly Random _random;
        private bool _isDisposed = false;

        public ImageFileService(IAlbumUnitOfWork db, MapperConfiguration mapperConfig, 
            Lazy<List<string>> allowedFileExtensions)
        {
            _db = db
                ?? throw new ArgumentNullException("UnitOfWork must be not null!");
            _mapper = mapperConfig?.CreateMapper()
                ?? throw new ArgumentNullException("Mapper configuration must be not null!");
            _random = new Random();
            _imageExtensions = allowedFileExtensions
                ?? throw new ArgumentNullException("Extensions must be not null!");
        }

        public int AddImageFile(ImageFileDTO file)
        {
            ValidateFile(file);
            if (GetFileByNameAndFolder(file.Name, file.FolderPath) == null)
                throw new ServiceException(ExceptionType.UniqueException, 
                    "Such image file already exists!");
            File fileDAL = _db.FileRepository.Add(_mapper.Map<File>(file));
            _db.Save();
            return fileDAL.Id;
        }

        public int GenerateImageFile(string fileName, string folderPath)
        {
            if (!(folderPath?.Trim().Length > 0))
                throw new ServiceException(ExceptionType.EmptyStringException, "Folder name is empty!");
            string extension = CheckFormatAndGetExtension(fileName);
            StringBuilder newImageName = new StringBuilder(MakeImageName(extension));
            while (GetFileByNameAndFolder(newImageName.ToString(), folderPath) != null)
                newImageName.Append(_random.Next(0, 10));
            File fileDAL = _db.FileRepository.Add(new File
            {
                Name = newImageName.ToString().Trim(),
                Folder = folderPath.Trim()
            });
            _db.Save();
            return fileDAL.Id;
        }

        public int UpdateOrCreateImageFile(ImageFileDTO file)
        {
            if (file == null)
                throw new ServiceException(ExceptionType.NullException, "Image file is null");
            var unique = GetFileByNameAndFolder(file.Name, file.FolderPath);
            if ((unique != null) && (unique.Id != file.Id))
                return unique.Id;
            File fileDAL = _db.FileRepository.Get(file.Id);
            if (fileDAL == null)
                return AddImageFile(file);
            ValidateFile(file);
            fileDAL.Folder = file.FolderPath;
            fileDAL.Name = file.Name;
            _db.Save();
            return fileDAL.Id;
        }

        public bool DeleteImageFileById(int fileId)
        {
            if (!_db.FileRepository.Contains(fileId))
                return false;
            _db.FileRepository.Delete(fileId);
            _db.Save();
            return true;
        }

        public HashSet<ImageFileDTO> GetAllImageFiles()
        {
            return _mapper.Map<IEnumerable<File>, HashSet<ImageFileDTO>>
                    (_db.FileRepository.GetAll().ToList());
        }

        public ImageFileDTO GetImageFileById(int fileId)
        {
            return _mapper.Map<ImageFileDTO>(_db.FileRepository.Get(fileId));
        }

        public ImageFileDTO GetImageFileByImageId(int imageId)
        {
            Image image = _db.ImageRepository.Get(imageId);
            if (image == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Image with id = {imageId} was not found!");
            return _mapper.Map<ImageFileDTO>(image.ImageFile);
        }

        public ImageFileDTO GetImageFileByProfileId(int profileId)
        {
            DAL.Entities.Profile profile = _db.ProfileRepository.Get(profileId);
            if (profile == null)
                throw new ServiceException(ExceptionType.NotFoundException,
                    $"Profile with id = {profileId} was not found!");
            return _mapper.Map<ImageFileDTO>(profile.ImageFile);
        }

        public string GetImagePath(int fileId)
        {
            ImageFileDTO file = GetImageFileById(fileId)
                ?? throw new ServiceException(ExceptionType.NotFoundException, 
                $"File with id = {fileId} was not found!");
            return $"{file.FolderPath}/{file.Name}";
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

        private void ValidateFile(ImageFileDTO imageFile)
        {
            if (imageFile == null)
                throw new ServiceException(ExceptionType.NullException, $"Image file is null!");
            if (!(imageFile.Name?.Length > 0))
                throw new ServiceException("File name is empty!")
                {
                    Type = ExceptionType.EmptyStringException
                };
            if (imageFile.FolderPath == null)
                throw new ServiceException("Folder name is empty!")
                {
                    Type = ExceptionType.EmptyStringException,
                    ExceptionValue = nameof(imageFile.FolderPath)
                };
            CheckFormatAndGetExtension(imageFile.Name);
        }

        private File GetFileByNameAndFolder(string name, string folderPath)
        {
            return _db.FileRepository.GetAll()
                .FirstOrDefault(x => (x.Folder == folderPath.Trim()) && (x.Name == name.Trim()));
        }

        private string CheckFormatAndGetExtension(string fileName)
        {
            if (fileName == null)
                throw new ServiceException(ExceptionType.EmptyStringException,
                    "Image file name is empty!");
            string extension = fileName.Substring(fileName.LastIndexOf('.'));
            if (!_imageExtensions.Value.Contains(extension.ToLower()))
                throw new ServiceException(ExceptionType.FileExtensionException,
                    $"Invalid image file format: {extension}");
            return extension;
        }
        
        private string MakeImageName(string extension)
        {
            if (!(extension?.Trim().Length > 0))
                throw new ServiceException(ExceptionType.FileExtensionException, 
                    $"Image file extension is missing!"); 
            DateTime time = DateTime.UtcNow;
            return $"{GetHashCode()}.{time.Year}.{time.Month}.{time.Day}" +
                $".{time.Hour}.{time.Minute}.{time.Second}{extension}";
        }
    }
}
