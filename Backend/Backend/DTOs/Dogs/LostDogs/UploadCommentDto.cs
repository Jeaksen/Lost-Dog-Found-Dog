using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Dogs
{
    public class UploadCommentDto
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public LocationDto LocationDto { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int LostDogId { get; set; }
    }
}
