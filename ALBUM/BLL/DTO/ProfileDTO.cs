using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public sealed class ProfileDTO : DTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ImageFileId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
