using Backend.Models.Dogs.LostDogs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs
{
    public class LocationDog : IComparable, IEquatable<LocationDog>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string District { get; set; }

        [Required]
        public int DogId { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is LocationDog location)
            {
                int result = location.City.CompareTo(City);
                if (result != 0)
                    return result;
                return location.District.CompareTo(District);
            }
            return -1;
        }

        public bool Equals(LocationDog other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString() => $"{City} - {District}";

    }
}
