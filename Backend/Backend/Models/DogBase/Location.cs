using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DogBase
{
    public class DogBehvaior
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string District { get; set; }

    }
}
