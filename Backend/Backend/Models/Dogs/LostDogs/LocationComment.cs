using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Dogs.LostDogs
{
    public class LocationComment : IComparable, IEquatable<LocationComment>
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
        public int CommentId { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is LocationComment location)
            {
                int result = location.City.CompareTo(City);
                if (result != 0)
                    return result;
                return location.District.CompareTo(District);
            }
            return -1;
        }

        public bool Equals(LocationComment other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString() => $"{City} - {District}";

    }
}
