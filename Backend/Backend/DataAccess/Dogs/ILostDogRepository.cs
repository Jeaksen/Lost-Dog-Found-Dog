using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dogs
{
    public interface ILostDogRepository
    {
        public Task<RepositoryResponse<LostDog>> AddLostDog(LostDog lostDog);
        public Task<RepositoryResponse<List<LostDog>>> GetLostDogs();
        public Task<RepositoryResponse<List<LostDog>>> GetUserLostDogs(int ownerId);
        public Task<RepositoryResponse<LostDog>> GetLostDogDetails(int dogId);
        public Task<RepositoryResponse<LostDog>> UpdateLostDog(LostDog updatedDog);
        public Task<RepositoryResponse<bool>> MarkDogAsFound(int dogId);
        public Task<RepositoryResponse<bool>> DeleteLostDog(int dogId);


        public Task<RepositoryResponse<LostDogComment>> AddLostDogComment(LostDogComment comment);
        public Task<RepositoryResponse<List<LostDogComment>>> GetLostDogComments(int dogId);
        public Task<RepositoryResponse<LostDogComment>> EditLostDogComment(LostDogComment comment);

    }
}
