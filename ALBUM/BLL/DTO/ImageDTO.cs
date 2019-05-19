using System.ComponentModel.DataAnnotations;
using System;

namespace BLL.DTO
{
    public sealed class ImageDTO : DTO
    {
        public string Title { get; set; }
        public DateTime? Time { get; set; }
        [Required]
        public int ProfileId { get; set; }
        public int? ImageFileId { get; set; }
    }
}
