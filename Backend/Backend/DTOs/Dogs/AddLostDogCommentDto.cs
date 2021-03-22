using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Dogs
{
    public class AddLostDogCommentDto
    {

        [Required]
        public int Text { get; set; }

        [Required]
        public int Location { get; set; }

        [Required]
        [Display(Name = "authorId")]
        public int AccountId { get; set; }

        [Required]
        public int DogId { get; set; }
    }
}
