using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs.Dogs
{
    public class GetLostDogDto : GetDogDto
    {
        public LocationDto Location { get; set; }

        public DateTime DateLost { get; set; }

        public bool IsFound { get; set; } = false;

        public int OwnerId { get; set; }

        public List<LostDogComment> Comments { get; set; }
    }
}
