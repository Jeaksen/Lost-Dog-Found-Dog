using Backend.DataAccess.ShelterDogs;
using Backend.Models.Dogs;
using Backend.Models.Dogs.ShelterDogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.ShelterDogs
{
    [Collection("Database collection")]
    public class ShelterDogRepositoryTests
    {
        private readonly IShelterDogRepository shelterDogRepository;

        public ShelterDogRepositoryTests(DatabaseFixture databaseAuthFixture)
        {
            shelterDogRepository = databaseAuthFixture.ShelterDogRepository;
        }

        private ShelterDog GetValidShelterDog()
        {
            return new ShelterDog()
            {
                Breed = "dogdog",
                Age = 5,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new Picture()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                ShelterId = 1
            };
        }

        [Fact]
        public async void AddingShelterDogSuccessfulForValidDogInfo()
        {
            var result = await shelterDogRepository.AddShelterDog(GetValidShelterDog());
            Assert.True(result.Successful);
        }

        [Fact]
        public async void AddingShelterDogFailsForMissingRequiredData()
        {
            var saveDogDog = GetValidShelterDog();
            saveDogDog.Name = null;
            var result = await shelterDogRepository.AddShelterDog(saveDogDog);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void GettingShelterDogDetailsForDogOneSuccessful()
        {
            var result1 = await shelterDogRepository.AddShelterDog(GetValidShelterDog());
            Assert.True(result1.Successful);
            var result2 = await shelterDogRepository.GetShelterDogDetails(result1.Data.Id);
            Assert.True(result2.Successful);
        }

        [Fact]
        public async void DeletingShelterDogClearsAllData()
        {
            var dog = await shelterDogRepository.AddShelterDog(GetValidShelterDog());
            Assert.True(dog.Successful);

            var result = await shelterDogRepository.DeleteShelterDog(dog.Data.Id);
            Assert.True(result.Successful);

            dog = await shelterDogRepository.GetShelterDogDetails(dog.Data.Id);
            Assert.False(dog.Successful);
        }

        [Fact]
        public async void GetDogsForValidShelterSuccessful()
        {
            var dog = await shelterDogRepository.AddShelterDog(GetValidShelterDog());
            Assert.True(dog.Successful);

            var result = await shelterDogRepository.GetShelterDogs(dog.Data.ShelterId, 0, 50);
            Assert.True(result.Successful && result.Data.Count > 0);
        }
    }
}
