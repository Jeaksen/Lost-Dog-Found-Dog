using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DogBase
{
    public class DogBehavior
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Behvaior { get; set; }

        [Required]
        public int DogId { get; set; }
    }
}
