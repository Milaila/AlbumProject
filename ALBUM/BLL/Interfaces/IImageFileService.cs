using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IImageFileService : IDisposable
    {
        int AddImageFile(ImageFileDTO file);
        int UpdateOrCreateImageFile(ImageFileDTO file);
        ImageFileDTO GetImageFileById(int fileId);
        ImageFileDTO GetImageFileByImageId(int imageId);
        ImageFileDTO GetImageFileByProfileId(int imageId);
        HashSet<ImageFileDTO> GetAllImageFiles();
        string GetImagePath(int fileId);
        bool DeleteImageFileById(int fileId);
        int GenerateImageFile(string fileName, string folderPath);
    }
}
