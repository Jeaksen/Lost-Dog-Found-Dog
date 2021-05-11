using Backend.DTOs.Authentication;
using Backend.DTOs.Shelters;
using Backend.Models.Response;
using System.Threading.Tasks;

namespace Backend.Services.Shelters
{
    public interface IShelterService
    {
        public Task<ServiceResponse<ShelterDto, GetAccountDto>> AddShelter(ShelterDto shelterDto);

        public Task<ServiceResponse<ShelterDto>> GetShelter(int id);

        public Task<ServiceResponse> DeleteShelter(int id);
    }
}
