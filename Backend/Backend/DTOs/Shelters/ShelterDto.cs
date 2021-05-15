using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Shelters
{
    public class ShelterDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        public AddressDto Address { get; set; }
    }
}
