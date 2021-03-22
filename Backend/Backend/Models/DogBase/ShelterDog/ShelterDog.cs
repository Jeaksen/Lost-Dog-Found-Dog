using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.DogBase.ShelterDog
{
    public class ShelterDog : Dog
    {
        public int ShelterId { get; set; }
    }
}
