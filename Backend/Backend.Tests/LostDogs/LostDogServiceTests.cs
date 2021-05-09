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
using Backend.DTOs.Dogs;
using Backend.Services.Security;
using Backend.Models.Response;

namespace Backend.Tests.LostDogs
{
    [Collection("Database collection")]
    public class LostDogServiceTests
    {
        private readonly ILogger<LostDogService> logger;
        private readonly IMapper mapper;

        public LostDogServiceTests(DatabaseFixture databaseAuthFixture)
        {
            logger = databaseAuthFixture.LoggerFactory.CreateLogger<LostDogService>();
            mapper = databaseAuthFixture.Mapper;
        }

        [Fact]
        public async void GetLostDogsSuccessfulForNotNullData()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();
            repo.Setup(o => o.GetLostDogs(It.IsAny<LostDogFilter>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<LostDog>, int>()));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.True((await service.GetLostDogs(new LostDogFilter(), null, 0 , 0)).Successful);
        }

        [Fact]
        public async void GetLostDogsFailsForNullData()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();
            repo.Setup(o => o.GetLostDogs(It.IsAny<LostDogFilter>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<LostDog>, int>() { Successful = false }));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.False((await service.GetLostDogs(new LostDogFilter(), null, 0, 0)).Successful);
        }

        [Fact]
        public async void GetLostDogsDetailsSuccessfulForExistingDog()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();
            repo.Setup(o => o.GetLostDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>()));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.True((await service.GetLostDogDetails(1)).Successful);
        }

        [Fact]
        public async void GetLostDogsDetailsFailsForNotExistingDog()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();
            repo.Setup(o => o.GetLostDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Successful = false }));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.False((await service.GetLostDogDetails(1)).Successful);
        }

        [Fact]
        public async void AddLostDogSuccessfulForValidPostDogWithImage()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();

            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            var dogDto = new UploadLostDogDto();
            var dog = mapper.Map<LostDog>(dogDto);
            repo.Setup(o => o.AddLostDog(It.IsAny<LostDog>())).Returns((LostDog d) => Task.FromResult(new RepositoryResponse<LostDog>() { Data = d }));
            security.Setup(s => s.IsPictureValid(It.IsAny<IFormFile>())).Returns((IFormFile f) => new ServiceResponse());
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.True((await service.AddLostDog(dogDto, picture)).Successful);
        }

        [Fact]
        public async void AddLostDogFailsForValidPostDogAndInvalidImage()
        {
            var security = new Mock<ISecurityService>();
            var repo = new Mock<ILostDogRepository>();
            var picture = new FormFile(null, 0, 0, "name", "filename");
            var dogDto = new UploadLostDogDto();
            var dog = mapper.Map<LostDog>(dogDto);
            repo.Setup(o => o.AddLostDog(dog)).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Data = dog }));
            security.Setup(s => s.IsPictureValid(It.IsAny<IFormFile>())).Returns((IFormFile f) => new ServiceResponse() { Successful = false});
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.False((await service.AddLostDog(dogDto, null)).Successful);
        }

        [Fact]
        public async void MarkLostDogSuccessfulForValidDog()
        {
            var security = new Mock<ISecurityService>();
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.MarkDogAsFound(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.True((await service.MarkLostDogAsFound(1)).Successful);
        }

        [Fact]
        public async void MarkLostDogFailsForInvalidDog()
        {
            var security = new Mock<ISecurityService>();
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.MarkDogAsFound(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse() { Successful = false }));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.False((await service.MarkLostDogAsFound(1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogSuccessfulForExistingDogNoPicture()
        {
            var security = new Mock<ISecurityService>();
            var repo = new Mock<ILostDogRepository>();
            repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>()));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.True((await service.UpdateLostDog(new UploadLostDogDto(), null, 1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogSuccessfulForExistingDogNewPicture()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();

            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            var dogDto = new UploadLostDogDto();
            var dog = mapper.Map<LostDog>(dogDto);
            repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns((LostDog d) => Task.FromResult(new RepositoryResponse<LostDog>() { Data = d }));
            security.Setup(s => s.IsPictureValid(It.IsAny<IFormFile>())).Returns((IFormFile f) => new ServiceResponse());
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.True((await service.UpdateLostDog(dogDto, picture, 1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogFailsForNonExistingDogNoPicture()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();
            repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>(){ Successful = false }));
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.False((await service.UpdateLostDog(new UploadLostDogDto(), null, 1)).Successful);
        }

        [Fact]
        public async void UpdatingLostDogFailsForNonExistingDogNewPicture()
        {
            var repo = new Mock<ILostDogRepository>();
            var security = new Mock<ISecurityService>();

            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            var dogDto = new UploadLostDogDto();
            var dog = mapper.Map<LostDog>(dogDto);
            repo.Setup(o => o.UpdateLostDog(It.IsAny<LostDog>())).Returns(Task.FromResult(new RepositoryResponse<LostDog>() { Successful = false }));
            security.Setup(s => s.IsPictureValid(It.IsAny<IFormFile>())).Returns((IFormFile f) => new ServiceResponse());
            var service = new LostDogService(repo.Object, security.Object, mapper, logger);

            Assert.False((await service.UpdateLostDog(dogDto, picture, 1)).Successful);
        }
    }
}
