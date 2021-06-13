using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Shelters
{
    public class AddressDto
    {
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string PostCode { get; set; }

        [Required]
        [MaxLength(20)]
        public string BuildingNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string additionalAddressLine { get; set; }
    }
}
