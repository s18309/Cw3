using System.ComponentModel.DataAnnotations;

namespace cw3.DTOs
{
    public class PromotionDTO
    {
        [Required]
        public int StudiesId { get; set; }
        [Required]
        public int Semester { get; set; }
    }
}