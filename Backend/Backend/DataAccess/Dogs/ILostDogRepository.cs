using Backend.DTOs.Dogs;
using Backend.Models.DogBase.LostDog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dogs
{
    interface ILostDogRepository
    {
        public List<LostDog> GetLostDogs();
        public LostDog GetUserLostDogs(int ownerId);
        public LostDog AddLostDog(LostDog lostDog);
        public LostDog GetLostDogDetails(int dogId);
        public bool DeleteLostDog(int dogId);

        public List<LostDogComment> GetLostDogComments(int dogId);
        public LostDogComment AddLostDogComment(LostDogComment comment, int dogId);
        public LostDogComment EditLostDogComment(LostDogComment comment, int dogId);

    }
}
