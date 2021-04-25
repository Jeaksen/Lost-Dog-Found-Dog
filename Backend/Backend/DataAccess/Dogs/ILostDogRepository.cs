using Backend.Models.DogBase.LostDog;
using Backend.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dogs
{
    public interface ILostDogRepository
    {
        public Task<RepositoryResponse<LostDog>> AddLostDog(LostDog lostDog);
        public Task<RepositoryResponse<List<LostDog>, int>> GetLostDogs(LostDogFilter filter, string sort, int page, int size);
        public Task<RepositoryResponse<LostDog>> GetLostDogDetails(int dogId);
        public Task<RepositoryResponse<LostDog>> UpdateLostDog(LostDog updatedDog);
        public Task<RepositoryResponse> MarkDogAsFound(int dogId);
        public Task<RepositoryResponse> DeleteLostDog(int dogId);


        public Task<RepositoryResponse<LostDogComment>> AddLostDogComment(LostDogComment comment);
        public Task<RepositoryResponse<List<LostDogComment>>> GetLostDogComments(int dogId);
        public Task<RepositoryResponse<LostDogComment>> EditLostDogComment(LostDogComment comment);
    }
}
