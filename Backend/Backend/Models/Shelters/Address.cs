using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Shelters
{
    public class Address
    {
        [Required]
        public int Id { get; set; }

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

        [MaxLength(200)]
        public string AdditionalAddressLine { get; set; }
    }
}
