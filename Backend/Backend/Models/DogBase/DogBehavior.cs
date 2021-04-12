using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models.DogBase
{
    public class DogBehavior : IEquatable<DogBehavior>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Behvaior { get; set; }

        [Required]
        public int DogId { get; set; }

        public bool Equals(DogBehavior other)
        {
            return Behvaior == other.Behvaior;
        }

        public override bool Equals(object obj) => Equals(obj as DogBehavior);
        public override int GetHashCode() => Behvaior.GetHashCode();
        public override string ToString() => Behvaior;
    }

    public class DogBehaviorComparer : IEqualityComparer<DogBehavior>
    {
        public bool Equals(DogBehavior x, DogBehavior y) => x.Behvaior == y.Behvaior;

        public int GetHashCode([DisallowNull] DogBehavior obj) => obj.Behvaior.GetHashCode();
    }
}
