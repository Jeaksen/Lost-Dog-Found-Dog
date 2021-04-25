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
        public Task<ServiceResponse<LostDog>> AddLostDog(AddLostDogDto lostDog, IFormFile picture);
        public Task<ServiceResponse<List<LostDog>>> GetLostDogs();
        public Task<ServiceResponse<List<LostDog>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size);
        public Task<ServiceResponse<List<LostDog>>> GetUserLostDogs(int ownerId);
        public Task<ServiceResponse<LostDog>> GetLostDogDetails(int dogId);
        public Task<ServiceResponse<LostDog>> UpdateLostDog(UpdateLostDogDto lostDogDto, IFormFile picture, int dogId);
        public Task<ServiceResponse<bool>> MarkLostDogAsFound(int dogId);
        public Task<ServiceResponse<bool>> DeleteLostDog(int dogId);

        public Task<ServiceResponse<LostDogComment>> AddLostDogComment(AddLostDogCommentDto comment);
        public Task<ServiceResponse<List<LostDogComment>>> GetLostDogComments(int dogId);
        public Task<ServiceResponse<LostDogComment>> EditLostDogComment(LostDogComment comment);
    }
}
