using Backend.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Dogs.LostDogs
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
        public Account Owner { get; set; }

        [Required]
        public List<LostDogComment> Comments { get; set; }
    }
}
