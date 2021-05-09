using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Dogs
{
    public class UploadLostDogDto : UploadDogDto
    {
        public int OwnerId { get; set; }

        [Required]
        public LocationDto Location { get; set; }

        [Required]
        public DateTime DateLost { get; set; }

    }
}
