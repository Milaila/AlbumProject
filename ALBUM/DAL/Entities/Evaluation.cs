using DAL.Abstracts;

namespace DAL.Entities
{
    public class Evaluation : Entity
    {
        public int Mark { get; set; }

        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
