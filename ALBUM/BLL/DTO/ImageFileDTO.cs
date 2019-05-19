using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public sealed class ImageFileDTO : DTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string FolderPath { get; set; }
    }
}
