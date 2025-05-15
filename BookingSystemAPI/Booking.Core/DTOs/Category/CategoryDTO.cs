using System.ComponentModel.DataAnnotations;

namespace Booking.Core.DTOs.Category
{
    public class CategoryDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;
    }
}
