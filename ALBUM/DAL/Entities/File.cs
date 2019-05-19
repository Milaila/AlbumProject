using DAL.Abstracts;

namespace DAL.Entities
{
    public class File : Entity
    {
        public string Name { get; set; }
        public string Folder { get; set; }
    }
}
