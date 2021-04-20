using Backend.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models.DogBase.LostDog
{
    public class LostDogComment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Text { get; set; }

        [Required]
        public int Location { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public Account Author { get; set; } 

        [Required]
        public int DogId { get; set; }

        [Required]
        public Picture Picture { get; set; }
    }
}
