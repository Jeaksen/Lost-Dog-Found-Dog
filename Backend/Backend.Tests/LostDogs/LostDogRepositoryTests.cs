using Backend.DataAccess.Dogs;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Backend.Tests.LostDogs
{
    [Collection("Database collection")]
    public class LostDogRepositoryTests
    {
        private readonly ILostDogRepository lostDogRepository;

        public LostDogRepositoryTests(DatabaseFixture databaseAuthFixture)
        {
            lostDogRepository = databaseAuthFixture.LostDogRepository;
        }

        [Fact]
        public async void AddingLostDogSuccessfulForValidDogInfo()
        {
            var saveDog = new LostDog()
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
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behvaior = "Angry" } },
                Location = new Location() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.NotNull(result);
        }

        [Fact]
        public async void AddingLostDogFailsForMissingRequiredData()
        {
            var saveDogDog = new LostDog()
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
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behvaior = "Angry" } },
                Location = new Location() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(saveDogDog);
            Assert.Null(result);
        }

        [InlineData(1)]
        [Theory]
        public async void GettingLostDogsForUserOneSuccessful(int userId)
        {
            var result = await lostDogRepository.GetUserLostDogs(userId);
            Assert.NotNull(result);
        }

        [InlineData(1)]
        [Theory]
        public async void GettingLostDogDetailsForDogOneSuccessful(int dogId)
        {
            var result = await lostDogRepository.GetLostDogDetails(dogId);
            Assert.NotNull(result);
        }

        [Fact]
        public async void DeletingLostDogClearsAllData()
        {
            var saveDog = new LostDog()
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
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behvaior = "Angry" } },
                Location = new Location() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };

            var dog = await lostDogRepository.AddLostDog(saveDog);
            Assert.NotNull(dog);

            var result = await lostDogRepository.DeleteLostDog(dog.Id);
            Assert.True(result);

            dog = await lostDogRepository.GetLostDogDetails(dog.Id);
            Assert.Null(dog);
        }

        [Fact]
        public async void MarkingLostDogAsFoundSuccessfulForExistingDog()
        {
            var savedDogs = await lostDogRepository.GetLostDogs();

            Assert.NotEmpty(savedDogs);
            Assert.True(await lostDogRepository.MarkDogAsFound(savedDogs.First().Id));
        }

        [Fact]
        public async void MarkingLostDogAsFoundFailsForNonExistingDog()
        {
            Assert.False(await lostDogRepository.MarkDogAsFound(-1));
        }

        [Fact]
        public async void MarkingLostDogAsFoundFailsForDogMarkedAsFound()
        {
            var saveDog = new LostDog()
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
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behvaior = "Angry" } },
                Location = new Location() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.NotNull(result);

            Assert.True(await lostDogRepository.MarkDogAsFound(saveDog.Id));
            Assert.False(await lostDogRepository.MarkDogAsFound(saveDog.Id));
        }
    }
}
