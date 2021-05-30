using System.Collections.Generic;

namespace Backend.DTOs.Dogs
{
    public class GetLostDogWithCommentsDto : GetLostDogDto
    {
        public List<GetCommentDto> Comments { get; set; }
    }
}
