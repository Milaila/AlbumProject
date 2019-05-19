using System.ComponentModel.DataAnnotations;
using System;

namespace WebAPI.Models
{
    public sealed class ProfilePostModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ImageFileId { get; set; }
    }
}
