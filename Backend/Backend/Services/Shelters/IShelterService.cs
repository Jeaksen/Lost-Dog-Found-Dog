using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.DTOs.Shelters;
using Backend.Models.Response;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services.Shelters
{
    public interface IShelterService
    {
        public Task<ServiceResponse<ShelterDto, GetAccountDto>> AddShelter(ShelterDto shelterDto);

        public Task<ServiceResponse<ShelterDto>> GetShelter(int id);

        public Task<ServiceResponse<List<ShelterDto>, int>> GetShelters(string name, string sort, int page, int size);

        public Task<ServiceResponse> DeleteShelter(int id);

        public Task<ServiceResponse<GetShelterDogDto>> AddShelterDog(UploadShelterDogDto shelterDogDto, IFormFile picture);
        public Task<ServiceResponse<List<GetShelterDogDto>, int>> GetShelterDogs(int shelterId, int page, int size);
        public Task<ServiceResponse<GetShelterDogDto>> GetShelterDogDetails(int dogId);
        public Task<ServiceResponse> DeleteShelterDog(int dogId);
    }
}
