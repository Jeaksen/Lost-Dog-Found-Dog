using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Dogs
{
    public class AddLostDogDto : AddDogDto
    {
        public int OwnerId { get; set; }

        [Required]
        public AddLocationDto Location { get; set; }

        [Required]
        public DateTime DateLost { get; set; }

    }
}
