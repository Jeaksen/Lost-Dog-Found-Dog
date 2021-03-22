using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Dogs
{
    public class AddLocationDto
    {
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string District { get; set; }
    }
}
