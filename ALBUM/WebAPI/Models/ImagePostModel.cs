using System.Collections.Generic;

namespace WebAPI.Models
{
    public sealed class ImagePostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double AvgMark { get; set; }
        public ICollection<string> HashTags { get; set; }
        public int? FileId { get; set; }
        public int ProfileId { get; set; }
    }
}
