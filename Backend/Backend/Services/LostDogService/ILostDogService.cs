using Backend.DTOs.Dogs;
using Backend.Models.DogBase.LostDog;
using Backend.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services.LostDogService
{
    public interface ILostDogService
    {
        public Task<ServiceResponse<GetLostDogDto>> AddLostDog(UploadLostDogDto lostDog, IFormFile picture);
        public Task<ServiceResponse<List<GetLostDogDto>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size);
        public Task<ServiceResponse<GetLostDogDto>> GetLostDogDetails(int dogId);
        public Task<ServiceResponse<GetLostDogDto>> UpdateLostDog(UploadLostDogDto lostDogDto, IFormFile picture, int dogId);
        public Task<ServiceResponse> MarkLostDogAsFound(int dogId);
        public Task<ServiceResponse> DeleteLostDog(int dogId);

        public Task<ServiceResponse<LostDogComment>> AddLostDogComment(AddLostDogCommentDto comment);
        public Task<ServiceResponse<List<LostDogComment>>> GetLostDogComments(int dogId);
        public Task<ServiceResponse<LostDogComment>> EditLostDogComment(LostDogComment comment);
    }
}
