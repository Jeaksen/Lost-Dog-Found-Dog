using System;
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
        public string Behavior { get; set; }

        [Required]
        public int DogId { get; set; }

        public bool Equals(DogBehavior other)
        {
            return Behavior == other.Behavior;
        }

        public override bool Equals(object obj) => Equals(obj as DogBehavior);
        public override int GetHashCode() => Behavior.GetHashCode();
        public override string ToString() => Behavior;
    }

    public class DogBehaviorComparer : IEqualityComparer<DogBehavior>
    {
        public bool Equals(DogBehavior x, DogBehavior y) => x.Behavior == y.Behavior;

        public int GetHashCode([DisallowNull] DogBehavior obj) => obj.Behavior.GetHashCode();
    }
}
