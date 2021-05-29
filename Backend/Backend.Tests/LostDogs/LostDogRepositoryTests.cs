using Backend.DataAccess.LostDogs;
using Backend.Models.Dogs;
using Backend.Models.Dogs.LostDogs;
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

        private LostDog GetValidDog()
        {
            return new LostDog()
            {
                Breed = "dogdog",
                Age = 5,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
        }

        private LostDogComment GetValidComment()
        {
            return new LostDogComment()
            {
                Text = "nice dog",
                AuthorId = 1,
                Location = new LocationComment()
                {
                    City = "Warsaw",
                    District = "Mokotów"
                },
                Picture = new PictureComment()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }

                }
            };
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
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
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
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(saveDogDog);
            Assert.False(result.Successful);
        }

        [InlineData(1)]
        [Theory]
        public async void GettingLostDogsForUserOneSuccessful(int userId)
        {
            var filter = new LostDogFilter() { OwnerId = userId };
            var result = await lostDogRepository.GetLostDogs(filter, null, 0, 10);
            Assert.True(result.Successful);
        }

        [InlineData(1)]
        [Theory]
        public async void GettingLostDogDetailsForDogOneSuccessful(int dogId)
        {
            var result = await lostDogRepository.GetLostDogDetails(dogId);
            Assert.True(result.Successful);
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
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };

            var dog = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(dog.Successful);

            var result = await lostDogRepository.DeleteLostDog(dog.Data.Id);
            Assert.True(result.Successful);

            dog = await lostDogRepository.GetLostDogDetails(dog.Data.Id);
            Assert.False(dog.Successful);
        }

        [Fact]
        public async void MarkingLostDogAsFoundSuccessfulForExistingDog()
        {
            var savedDogs = await lostDogRepository.GetLostDogs(new LostDogFilter(), null, 0, 10);

            Assert.True(savedDogs.Successful);
            Assert.NotEmpty(savedDogs.Data);
            var response = await lostDogRepository.MarkDogAsFound(savedDogs.Data.First().Id);
            Assert.True(response.Successful);
        }

        [Fact]
        public async void MarkingLostDogAsFoundFailsForNonExistingDog()
        {
            var response = await lostDogRepository.MarkDogAsFound(-1);
            Assert.False(response.Successful);
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
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.NotNull(result);
            var response = await lostDogRepository.MarkDogAsFound(saveDog.Id);
            Assert.True(response.Successful);
            response = await lostDogRepository.MarkDogAsFound(saveDog.Id);
            Assert.False(response.Successful);
        }
    
        [Fact]
        public async void UpdatingLostDogSuccessfulForExistingDog()
        {
            var lostDog = new LostDog()
            {
                Breed = "dogdog",
                Age = 5,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(lostDog);
            Assert.True(result.Successful);
            lostDog = result.Data;
            lostDog.Age = 6;
            Assert.True((await lostDogRepository.UpdateLostDog(lostDog)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogFailsForNonExistingDog()
        {
            var lostDog = new LostDog { Id = -1 };
            Assert.False((await lostDogRepository.UpdateLostDog(lostDog)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogSuccessfulForInvalidData()
        {
            var lostDog = new LostDog()
            {
                Breed = "dogdog",
                Age = 5,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var result = await lostDogRepository.AddLostDog(lostDog);
            Assert.True(result.Successful);
            lostDog = result.Data;
            lostDog.Breed = null;
            Assert.False((await lostDogRepository.UpdateLostDog(lostDog)).Successful);
        }

        [Fact]
        public async void GetLostDogSortsProperly()
        {
            var saveDog = new LostDog()
            {
                Breed = "dogdog",
                Age = 5,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Biała", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            var saveDog2 = new LostDog()
            {
                Breed = "dogdog",
                Age = 6,
                Size = "Large, very large",
                Color = "Orange but a bit yellow and green dots",
                SpecialMark = "tattoo of you on the neck",
                Name = "Cat",
                Picture = new PictureDog()
                {
                    FileName = "photo",
                    FileType = "png",
                    Data = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
                },
                HairLength = "Long",
                EarsType = "Short",
                TailLength = "None",
                Behaviors = new List<DogBehavior>() { new DogBehavior() { Behavior = "Angry" } },
                Location = new LocationDog() { City = "Czarna", District = "Lol ther's none" },
                DateLost = new DateTime(2021, 3, 20),
                OwnerId = 1,
                Comments = new List<LostDogComment>()
            };
            Assert.True((await lostDogRepository.AddLostDog(saveDog)).Successful);
            Assert.True((await lostDogRepository.AddLostDog(saveDog2)).Successful);

            var filteringResult  = await lostDogRepository.GetLostDogs(new LostDogFilter(), "location.city", 0, 50);
            var filteringResult2 = await lostDogRepository.GetLostDogs(new LostDogFilter(), "Location.City,ASC", 0, 50);
            var filteringResult3 = await lostDogRepository.GetLostDogs(new LostDogFilter(), "location.city,DESC", 0, 50);
            var filteringResult4 = await lostDogRepository.GetLostDogs(new LostDogFilter(), "location.city,deSc", 0, 50);
            var filteringResult5 = await lostDogRepository.GetLostDogs(new LostDogFilter(), "notexsitant,DESC", 0, 50);

            Assert.True(filteringResult.Successful && filteringResult2.Successful && filteringResult3.Successful && filteringResult4.Successful && !filteringResult5.Successful);
            Assert.True(filteringResult.Data[0].Id == filteringResult2.Data[0].Id && filteringResult.Data[^1].Id == filteringResult3.Data[0].Id && filteringResult3.Data[0].Id == filteringResult4.Data[0].Id);
        }

        [Fact]
        public async void AddLostDogCommentSuccessfulForValidCommentAndExistingDog()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.True(commentResult.Successful);
        }

        [Fact]
        public async void AddLostDogCommentFailsForValidCommentAndNotExistingDog()
        {
            var comment = GetValidComment();
            comment.LostDogId = -1;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.False(commentResult.Successful);
        }

        [Fact]
        public async void AddLostDogCommentSuccessfulForCommentWithoutPicture()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            comment.Picture = null;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.True(commentResult.Successful);
        }

        [Fact]
        public async void AddLostDogCommentFailsForCommentWithoutLocation()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            comment.Location = null;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.False(commentResult.Successful);
        }

        [Fact]
        public async void AddLostDogCommentFailsForCommentWithoutText()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            comment.Text = null;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.False(commentResult.Successful);
        }

        [Fact]
        public async void AddLostDogCommentFailsForCommentWithNonExistingAuthor()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            comment.AuthorId = -1;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.False(commentResult.Successful);
        }

        [Fact]
        public async void DeleteLostDogCommentSuccessfulForExistingDogAndComment()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.True(commentResult.Successful);
            var removeResult = await lostDogRepository.DeleteLostDogComment(commentResult.Data.LostDogId, commentResult.Data.Id);
            Assert.True(removeResult.Successful);
        }

        [Fact]
        public async void DeleteLostDogCommentFailsForNotExistingDog()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.True(commentResult.Successful);
            var removeResult = await lostDogRepository.DeleteLostDogComment(-1, commentResult.Data.Id);
            Assert.False(removeResult.Successful);
        }

        [Fact]
        public async void DeleteLostDogCommentFailsForNotExistingComment()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.True(commentResult.Successful);
            var removeResult = await lostDogRepository.DeleteLostDogComment(commentResult.Data.LostDogId, -1);
            Assert.False(removeResult.Successful);
        }

        [Fact]
        public async void DeleteLostDogCommentFailsForCommentAddedToAnotherDog()
        {
            var saveDog = GetValidDog();
            var result = await lostDogRepository.AddLostDog(saveDog);
            Assert.True(result.Successful);
            var saveDog2 = GetValidDog();
            var result2 = await lostDogRepository.AddLostDog(saveDog2);
            Assert.True(result2.Successful);
            var comment = GetValidComment();
            comment.LostDogId = result.Data.Id;
            var commentResult = await lostDogRepository.AddLostDogComment(comment);
            Assert.True(commentResult.Successful);
            var removeResult = await lostDogRepository.DeleteLostDogComment(result2.Data.Id, commentResult.Data.Id);
            Assert.False(removeResult.Successful);
        }
    }
}
