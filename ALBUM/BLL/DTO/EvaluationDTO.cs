using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public sealed class EvaluationDTO : DTO
    {
        public const int MaxMark = 10;
        public const int MinMark = 1;

        [Required]
        public int Mark { get; set; }
        [Required]
        public int ImageId { get; set; }
        [Required]
        public int ProfileId { get; set; }
    }
}
