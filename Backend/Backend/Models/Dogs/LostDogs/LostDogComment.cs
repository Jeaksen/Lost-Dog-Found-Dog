using Backend.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs.LostDogs
{
    public class LostDogComment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Text { get; set; }

        [Required]
        public int Location { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public Account Author { get; set; } 

        [Required]
        public LostDog LostDog { get; set; }

        [Required]
        public int LostDogId { get; set; }

        [Required]
        public PictureComment Picture { get; set; }
    }
}
