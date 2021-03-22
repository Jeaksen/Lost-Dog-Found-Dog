using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DogBase
{
    public class Picture
    {
        [Required]
        [MaxLength(50)]
        public string FileName { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FileType { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
