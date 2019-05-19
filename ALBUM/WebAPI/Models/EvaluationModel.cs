using System.ComponentModel.DataAnnotations;
using BLL.DTO;

namespace WebAPI.Models
{
    public sealed class EvaluationModel
    {
        [Required]
        public int ImageId { get; set; }

        [Range(EvaluationDTO.MinMark, EvaluationDTO.MaxMark)]
        public int Mark { get; set; }
    }
}
