using Backend.DTOs.Authentication;
using Backend.Models.Dogs;
using Backend.Models.Dogs.LostDogs;

namespace Backend.DTOs.Dogs
{
    public class GetCommentDto
    {
        public int Id { get; set; }

        public int DogId { get; set; }

        public string Text { get; set; }

        public LocationDto Location { get; set; }

        public GetAccountDto Author { get; set; }

        public GetPictureDto Picture { get; set; }
    }
}
