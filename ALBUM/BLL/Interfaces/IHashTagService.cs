using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IHashTagService : IDisposable
    {
        int AddHashTag(HashTagDTO hashTag);
        HashSet<int> PutHashTags(HashSet<HashTagDTO> hashTags);
        HashSet<int> PutHashTagsByName(HashSet<string> hashTags);
        HashTagDTO GetHashTagById(int hashTagId);
        HashTagDTO GetHashTagByName(string hashTagName);
        HashSet<HashTagDTO> GetHashTagsByPartOfName(string name);
        HashSet<HashTagDTO> GetHashTagsByImage(int imageId);
        HashSet<HashTagDTO> GetAllHashTags();
        int PutHashTag(HashTagDTO hashTag);
        bool DeleteHashTagById(int hashTagId);
    }
}
