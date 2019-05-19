using System;
using DAL.Abstracts;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Image : Entity
    {
        public string Title { get; set; }
        public DateTime Time { get; set; }

        public virtual ICollection<Evaluation> Rating { get; set; }
        public virtual ICollection<HashTag> HashTags { get; set; }

        public int? ImageFileId { get; set; }
        public virtual File ImageFile { get; set; }
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public Image()
        {
            Rating = new HashSet<Evaluation>();
            HashTags = new HashSet<HashTag>();
        }
    }
}
