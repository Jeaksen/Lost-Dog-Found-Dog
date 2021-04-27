using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.DogBase.LostDog
{
    public class LostDogFilter
    {
        [MaxLength(50)]
        public string Breed { get; set; }

        [Range(0 , int.MaxValue)]
        public int? AgeFrom { get; set; }

        [Range(0, int.MaxValue)] 
        public int? AgeTo { get; set; }

        [MaxLength(50)]
        public string Size { get; set; }

        [MaxLength(50)]
        public string Color { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public int? OwnerId { get; set; }

        public LocationFilter Location { get; set; }

        public DateTime? DateLostBefore { get; set; }

        public DateTime? DateLostAfter { get; set; }

    }

    public class LocationFilter
    {
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string District { get; set; }
    }
}
