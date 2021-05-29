using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs
{
    public abstract class Dog
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Breed { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [MaxLength(50)]
        public string Size { get; set; }
        [Required]
        [MaxLength(100)]
        public string Color { get; set; }
        [Required]
        [MaxLength(100)]
        public string SpecialMark { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public PictureDog Picture { get; set; }
        [Required]
        [MaxLength(50)]
        public string HairLength { get; set; }
        [Required]
        [MaxLength(50)]
        public string EarsType { get; set; }
        [Required]
        [MaxLength(50)]
        public string TailLength { get; set; }
        [Required]
        public List<DogBehavior> Behaviors { get; set; }
    }
}
