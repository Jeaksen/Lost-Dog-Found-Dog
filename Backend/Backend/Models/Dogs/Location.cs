using Backend.Models.Dogs.LostDogs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs
{
    public class Location : IComparable, IEquatable<Location>
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
        public LostDog LostDog { get; set; }

        [Required]
        public int LostDogId { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is Location location)
            {
                int result = location.City.CompareTo(City);
                if (result != 0)
                    return result;
                return location.District.CompareTo(District);
            }
            return -1;
        }

        public bool Equals(Location other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString() => $"{City} - {District}";

    }
}
