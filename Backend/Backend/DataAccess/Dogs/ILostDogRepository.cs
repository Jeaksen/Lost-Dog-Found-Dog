using Backend.DTOs.Dogs;
using Backend.Models.DogBase.LostDog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dogs
{
    public interface ILostDogRepository
    {
        public Task<List<LostDog>> GetLostDogs();
        public Task<List<LostDog>> GetUserLostDogs(int ownerId);
        public Task<LostDog> AddLostDog(LostDog lostDog);
        public Task<LostDog> GetLostDogDetails(int dogId);
        public Task<bool> DeleteLostDog(int dogId);
        public Task<bool> MarkDogAsFound(int dogId);


        public Task<List<LostDogComment>> GetLostDogComments(int dogId);
        public Task<LostDogComment> AddLostDogComment(LostDogComment comment);
        public Task<LostDogComment> EditLostDogComment(LostDogComment comment);

    }
}
