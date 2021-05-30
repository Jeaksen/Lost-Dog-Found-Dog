using Backend.Models.Dogs.ShelterDogs;
using Backend.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.DataAccess.ShelterDogs
{
    public interface IShelterDogRepository
    {
        public Task<RepositoryResponse<ShelterDog>> AddShelterDog(ShelterDog shelterDog);
        public Task<RepositoryResponse<List<ShelterDog>, int>> GetShelterDogs(int shelterId, int page, int size);
        public Task<RepositoryResponse<ShelterDog>> GetShelterDogDetails(int dogId);
        public Task<RepositoryResponse> DeleteShelterDog(int dogId);
    }
}
