using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public sealed class HashTagDTO : DTO
    {
        [Required]
        public string Name { get; set; }
    }
}
