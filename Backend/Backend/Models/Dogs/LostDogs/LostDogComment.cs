using Backend.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs.LostDogs
{
    public class LostDogComment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public Account Author { get; set; }

        [Required]
        public int DogId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public LocationComment Location { get; set; }

        [Required]
        public PictureComment Picture { get; set; }
    }
}
