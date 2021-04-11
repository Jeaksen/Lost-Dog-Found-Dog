using Backend.Services.LostDogService;
using Moq;
using Backend.DataAccess.Dogs;
using Xunit;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models.DogBase.LostDog;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using Backend.DTOs.Dogs;
using Backend.DataAccess;

namespace Backend.Tests.LostDogs
{
    [Collection("Database collection")]
    public class LostDogServiceTests
    {
        private ILogger<LostDogService> logger;
        private IMapper mapper;

        public LostDogServiceTests(DatabaseFixture databaseAuthFixture)
        {
            logger = databaseAuthFixture.LoggerFactory.CreateLogger<LostDogService>();
            mapper = databaseAuthFixture.Mapper;
        }

        [Fact]
        public async void GetLostDogsSuccessfulForNotNullData()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetLostDogs()).Returns(Task.FromResult(new RepositoryResponse<List<LostDog>>()));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.True((await service.GetLostDogs()).Successful);
        }

        [Fact]
        public async void GetLostDogsFailsForNullData()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetLostDogs()).Returns(Task.FromResult(new RepositoryResponse<List<LostDog>>() { Successful = false }));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.False((await service.GetLostDogs()).Successful);
        }

        [Fact]
        public async void GetLostDogsForUserSuccessfulForNotNullData()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetUserLostDogs(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<LostDog>>()));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.True((await service.GetUserLostDogs(1)).Successful);
        }

        [Fact]
        public async void GetLostDogsForUserFailsForNullData()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetUserLostDogs(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<LostDog>>() { Successful = false }));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.False((await service.GetUserLostDogs(1)).Successful);
        }

        [Fact]
        public async void GetLostDogsDetailsSuccessfulForExistingDog()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetLostDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>()));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.True((await service.GetLostDogDetails(1)).Successful);
        }

        [Fact]
        public async void GetLostDogsDetailsFailsForNotExistingDog()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetLostDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Successful = false }));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.False((await service.GetLostDogDetails(1)).Successful);
        }

        [Fact]
        public async void AddLostDogSuccessfulForValidPostDogWithImage()
        {
            var repo = new Mock<ILostDogRepository>();
            using (var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 }))
            {
                var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                var dogDto = new AddLostDogDto();
                var dog = mapper.Map<LostDog>(dogDto);
                repo.Setup(o => o.AddLostDog(It.IsAny<LostDog>())).Returns((LostDog d) => Task.FromResult(new RepositoryResponse<LostDog>() {Data = d }));
                var service = new LostDogService(repo.Object, mapper, logger);

                Assert.True((await service.AddLostDog(dogDto, picture)).Successful);
            }
        }

        [Fact]
        public async void AddLostDogFailsForValidPostDogAndInvalidImage()
        {
            var repo = new Mock<ILostDogRepository>();
            var picture = new FormFile(null, 0, 0, "name", "filename");
            var dogDto = new AddLostDogDto();
            var dog = mapper.Map<LostDog>(dogDto);
            repo.Setup(o => o.AddLostDog(dog)).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Data = dog }));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.False((await service.AddLostDog(dogDto, null)).Successful);
        }

        [Fact]
        public async void MarkLostDogSuccessfulForValidDog()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.MarkDogAsFound(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<bool>()));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.True((await service.MarkLostDogAsFound(1)).Successful);
        }

        [Fact]
        public async void MarkLostDogFailsForInvalidDog()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.MarkDogAsFound(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<bool>() { Successful = false }));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.False((await service.MarkLostDogAsFound(1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogSuccessfulForExistingDogNoPicture()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetLostDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Data = new LostDog() }));
            repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>()));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.True((await service.UpdateLostDog(new UpdateLostDogDto(), null, 1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogSuccessfulForExistingDogNewPicture()
        {
            var repo = new Mock<ILostDogRepository>();

            using (var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 }))
            {
                var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                var dogDto = new UpdateLostDogDto();
                var dog = mapper.Map<LostDog>(dogDto);
                repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns((LostDog d) => Task.FromResult(new RepositoryResponse<LostDog>() { Data = d }));
                var service = new LostDogService(repo.Object, mapper, logger);

                Assert.True((await service.UpdateLostDog(dogDto, picture, 1)).Successful);
            }
        }

        [Fact]
        public async void UpdatingLostDogFailsForNonExistingDogNoPicture()
        {
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.GetLostDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Successful = false }));
            repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>()));
            var service = new LostDogService(repo.Object, mapper, logger);

            Assert.False((await service.UpdateLostDog(new UpdateLostDogDto(), null, 1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogFailsForNonExistingDogNewPicture()
        {
            var repo = new Mock<ILostDogRepository>();

            using (var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 }))
            {
                var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                var dogDto = new UpdateLostDogDto();
                var dog = mapper.Map<LostDog>(dogDto);
                repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Successful = false }));
                var service = new LostDogService(repo.Object, mapper, logger);

                Assert.False((await service.UpdateLostDog(dogDto, picture, 1)).Successful);
            }
        }
    }
}
