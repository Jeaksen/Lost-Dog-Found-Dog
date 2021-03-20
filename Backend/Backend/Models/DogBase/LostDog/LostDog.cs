using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DogBase.LostDog
{
    public class LostDog : Dog
    {
        [Required]
        public DogBehvaior Location { get; set; }
        [Required]
        public DateTime DateLost { get; set; }
        [Required]
        public bool IsFound { get; set; }
        [Required]
        public int OwnerId { get; set; }
    }
}
