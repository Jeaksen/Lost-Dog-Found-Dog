using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DogBase.LostDog
{
    public class LostDog : Dog
    {
        [Required]
        public Location Location { get; set; }

        [Required]
        public DateTime DateLost { get; set; }

        [Required]
        public bool IsFound { get; set; } = false;

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public List<LostDogComment> Comments { get; set; }
    }
}
