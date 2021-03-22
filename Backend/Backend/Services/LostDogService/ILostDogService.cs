using Backend.DTOs.Dogs;
using Backend.Models.DogBase.LostDog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services.LostDogService
{
    public interface ILostDogService
    {
        public Task<ServiceResponse<List<LostDog>>> GetLostDogs();
        public Task<ServiceResponse<List<LostDog>>> GetUserLostDogs(int ownerId);
        public Task<ServiceResponse<LostDog>> AddLostDog(AddLostDogDto lostDog, IFormFile image);
        public Task<ServiceResponse<LostDog>> GetLostDogDetails(int dogId);
        public Task<ServiceResponse<bool>> DeleteLostDog(int dogId);

        public Task<ServiceResponse<List<LostDogComment>>> GetLostDogComments(int dogId);
        public Task<ServiceResponse<LostDogComment>> AddLostDogComment(AddLostDogCommentDto comment);
        public Task<ServiceResponse<LostDogComment>> EditLostDogComment(LostDogComment comment);
    }
}
