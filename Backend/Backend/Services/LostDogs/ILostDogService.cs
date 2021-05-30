using Backend.DTOs.Dogs;
using Backend.Models.Dogs.LostDogs;
using Backend.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services.LostDogs
{
    public interface ILostDogService
    {
        public Task<ServiceResponse<GetLostDogDto>> AddLostDog(UploadLostDogDto lostDog, IFormFile picture);
        public Task<ServiceResponse<List<GetLostDogDto>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size);
        public Task<ServiceResponse<GetLostDogWithCommentsDto>> GetLostDogDetails(int dogId);
        public Task<ServiceResponse<GetLostDogDto>> UpdateLostDog(UploadLostDogDto lostDogDto, IFormFile picture, int dogId);
        public Task<ServiceResponse> MarkLostDogAsFound(int dogId);
        public Task<ServiceResponse> DeleteLostDog(int dogId);

        public Task<ServiceResponse<GetCommentDto>> AddLostDogComment(UploadCommentDto comment, IFormFile picture);
        public Task<ServiceResponse> DeleteLostDogComment(int dogId, int commentId);
        public Task<ServiceResponse<GetCommentDto>> GetLostDogComment(int dogId, int commentId);
    }
}
