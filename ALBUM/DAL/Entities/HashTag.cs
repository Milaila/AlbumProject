using DAL.Abstracts;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class HashTag : Entity
    {
        public string Name { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public HashTag()
            => Images = new HashSet<Image>();
    }
}
