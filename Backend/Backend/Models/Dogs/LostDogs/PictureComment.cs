using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs.LostDogs
{
    public class PictureComment
    {
        [Required]
        [MaxLength(150)]
        public string FileName { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FileType { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public LostDogComment LostDogComment { get; set; }

        public int LostDogCommentId { get; set; }

        public override string ToString() => FileName;
    }
}
