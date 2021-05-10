using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Shelters
{
    public class Shelter
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Address Address { get; set; }

        public int AddressId { get; set; }
    }
}
