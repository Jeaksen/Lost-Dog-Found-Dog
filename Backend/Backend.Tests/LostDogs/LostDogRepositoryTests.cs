using Backend.DataAccess.Dogs;
using Backend.Models.DogBase;
using Backend.Models.DogBase.LostDog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.LostDogs
{
    [Collection("Database collection")]
    public class LostDogRepositoryTests
    {
        private readonly DatabaseFixture _databaseAuthFixture;
        private readonly ILostDogRepository _lostDogRepository;

        public LostDogRepositoryTests(DatabaseFixture databaseAuthFixture)
        {
            _databaseAuthFixture = databaseAuthFixture;
            _lostDogRepository = databaseAuthFixture.LostDogRepository;
        }

        //[Fact]
        //public async void AddingLostDogSuccessfulForValidDogInfo()
        //{
        //    var saveDogDog = new LostDog()
        //    {
        //        Breed = "dogdog",
        //        Age = 5,
        //        Size = "Large, very large",
        //        Color = "Orange but a bit yellow and green dots",
        //        SpecialMark = "tattoo of you on the neck",
        //        Name = "Cat",
        //        Picture = new Picture()
        //            {
        //                FileName = "photo",
        //                FileType = "png",
        //                Data = new byte[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        //            },
        //        HairLength = "Long",
        //        EarsType = "Short",
        //        TailLength = "None",
        //        Behaviors = new List<DogBehavior>() { new DogBehavior() { Behvaior = "Angry"} },
        //        Location = new Location() { City = "Biała", District = "Lol ther's none"},
        //        DateLost = new DateTime(2021,3,20),
        //        OwnerId = 1,
        //        Comments = new List<LostDogComment>()
        //    };
        //    var result = await _databaseAuthFixture.LostDogRepository.AddLostDog(saveDogDog);
        //    Assert.NotNull(result);
        //}

        //[Fact]
        //public async void AddingLostDogFailsForMissingRequiredData()
        //{
        //    var saveDogDog = new LostDog()
        //    {
        //        Breed = "dogdog",
        //        Age = 5,
        //        Size = "Large, very large",
        //        Color = "Orange but a bit yellow and green dots",
        //        SpecialMark = "tattoo of you on the neck",
        //        Name = "Cat",
        //        Picture = new Picture()
        //        {
        //            FileName = "photo",
        //            FileType = "png",
        //            Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        //        },
        //        HairLength = "Long",
        //        TailLength = "None",
        //        Behaviors = new List<DogBehavior>() { new DogBehavior() { Behvaior = "Angry" } },
        //        Location = new Location() { City = "Biała", District = "Lol ther's none" },
        //        DateLost = new DateTime(2021, 3, 20),
        //        OwnerId = 1,
        //        Comments = new List<LostDogComment>()
        //    };
        //    var result = await _databaseAuthFixture.LostDogRepository.AddLostDog(saveDogDog);
        //    Assert.Null(result);
        //}

        //[InlineData(1)]
        //[Theory]
        //public async void GettingLostDogsForUserOneSuccessful(int userId)
        //{
        //    var result = await _databaseAuthFixture.LostDogRepository.GetUserLostDogs(userId);
        //    Assert.NotNull(result);
        //}
    }
}
