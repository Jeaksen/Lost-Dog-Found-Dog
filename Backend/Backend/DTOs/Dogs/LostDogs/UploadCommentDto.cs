using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Dogs
{
    public class UploadCommentDto
    {
        [Required]
        public int Text { get; set; }

        [Required]
        public int LocationDto { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int LostDogId { get; set; }
    }
}
