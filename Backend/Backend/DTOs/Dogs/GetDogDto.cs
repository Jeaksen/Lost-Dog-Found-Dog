using Backend.Models.Dogs;
using System.Collections.Generic;

namespace Backend.DTOs.Dogs
{
    public abstract class GetDogDto
    {
        public int Id { get; set; }

        public string Breed { get; set; }

        public int Age { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string SpecialMark { get; set; }

        public string Name { get; set; }

        public GetPictureDto Picture { get; set; }

        public string HairLength { get; set; }

        public string EarsType { get; set; }

        public string TailLength { get; set; }

        public List<string> Behaviors { get; set; }
    }
}
