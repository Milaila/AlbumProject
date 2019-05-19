using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IImageService : IDisposable
    {
        int AddImage(ImageDTO image, HashSet<int> hashTags = null);
        ImageDTO GetImageById(int imageId);
        HashSet<ImageDTO> GetAllImages();
        HashSet<ImageDTO> GetImagesByProfile(int profileId);
        HashSet<ImageDTO> GetImagesByHashTag(int hashTagId);
        HashSet<ImageDTO> GetImagesByTitle(string subTitle);
        HashSet<ImageDTO> GetImagesByHashTagAndProfile(int hashTagId, int profileId);
        HashSet<ImageDTO> GetImagesByTitleAndProfile(string subTitle, int profileId);
        int UpdateOrCreateImage(ImageDTO image, HashSet<int> hashTags);
        bool DeleteImageById(int imageId);
        void AddHashTagToImage(int imageId, int hashTagId);
        void PutHashTagsToImage(int imageId, HashSet<int> hashTags);
        double GetAverageMarkByImage(int imageId);
    }
}
