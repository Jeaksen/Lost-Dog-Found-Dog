using AutoMapper;
using Backend.DataAccess.ShelterDogs;
using Backend.DataAccess.Shelters;
using Backend.DTOs.Authentication;
using Backend.DTOs.Dogs;
using Backend.DTOs.Shelters;
using Backend.Models.Dogs.ShelterDogs;
using Backend.Models.Response;
using Backend.Models.Shelters;
using Backend.Services.Authentication;
using Backend.Services.Security;
using Backend.Services.Shelters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Shelters
{
    [Collection("Database collection")]
    public class ShelterServiceTests
    {

        private readonly ILogger<ShelterService> logger;
        private readonly IMapper mapper;

        public ShelterServiceTests(DatabaseFixture databaseAuthFixture)
        {
            logger = databaseAuthFixture.LoggerFactory.CreateLogger<ShelterService>();
            mapper = databaseAuthFixture.Mapper;
        }

        [Fact]
        public async void GetSheltersSuccessfulForRepoSucess()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(o => o.GetShelters(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<Shelter>, int>()));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.GetShelters(null, null, 0, 0)).Successful);
        }

        [Fact]
        public async void GetSheltersFailsForReporError()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(o => o.GetShelters(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<Shelter>, int>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.GetShelters(null, null, 0, 0)).Successful);
        }

        [Fact]
        public async void AddShelterSuccessfulForSuccessfulRepoAndAccountServiceResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter()}));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() {Data = new GetAccountDto() }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterFailsForFailedRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false}));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterFailsForFailedAccountServiceResponseAndSuccessfulShelterDelete()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterFailsForFailedAccountServiceResponseAndFailedShelterDelete()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse() { Successful = false}));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterWaitingForApprovalSuccessfulForSuccessfulRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.AddShelterWaitingForApproval(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterWaitingForApprovalFailsForFailedRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.AddShelterWaitingForApproval(new ShelterDto())).Successful);
        }

        [Fact]
        public async void ApproveShelterSuccessfulForSuccessfulRepoAndAccountResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.ApproveShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() {Data = new GetAccountDto() }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.ApproveShelter(1)).Successful);
        }

        [Fact]
        public async void ApproveShelterFailsForFailedRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.ApproveShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.ApproveShelter(1)).Successful);
        }

        [Fact]
        public async void ApproveShelterFailsForFailedAccountResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.ApproveShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.ApproveShelter(1)).Successful);
        }

        [Fact]
        public async void RejectShelterSuccessfulForSuccessfulRepoResponses()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.RejectShelter(1)).Successful);
        }

        [Fact]
        public async void RejectShelterFailsForFailedGetRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.RejectShelter(1)).Successful);
        }

        [Fact]
        public async void RejectShelterFailsForFailedDeleteRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse() { Successful = false }));
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.RejectShelter(1)).Successful);
        }

        [Fact]
        public async void GetShelterSuccessfulForSuccessRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter () }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.GetShelter(1)).Successful);
        }

        [Fact]
        public async void GetShelterFailsForFailedRepoResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.GetShelter(-1)).Successful);
        }

        [Fact]
        public async void DeleteShelterSuccessfulForSuccessResponses()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            account.Setup(s => s.DeleteAccount(null, It.IsAny<string>(), null)).Returns(Task.FromResult(new ServiceResponse()));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.DeleteShelter(1)).Successful);
        }

        [Fact]
        public async void DeleteShelterFailsForFailedGetShelterResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.DeleteShelter(-1)).Successful);
        }

        [Fact]
        public async void DeleteShelterFailsForFailedDeleteShelterResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse() { Successful = false }));
            account.Setup(s => s.DeleteAccount(null, It.IsAny<string>(), null)).Returns(Task.FromResult(new ServiceResponse()));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.DeleteShelter(-1)).Successful);
        }

        [Fact]
        public async void DeleteShelterFailsForFailedAccountDeleteResponse()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterRepo.Setup(r => r.GetShelterApprovalInvariant(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            shelterRepo.Setup(r => r.DeleteShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            account.Setup(s => s.DeleteAccount(null, It.IsAny<string>(), null)).Returns(Task.FromResult(new ServiceResponse() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.DeleteShelter(-1)).Successful);
        }

        [Fact]
        public async void GetLostDogsSuccessfulForRepoSucess()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterDogRepo.Setup(o => o.GetShelterDogs(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<ShelterDog>, int>()));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.GetShelterDogs(1, 0, 0)).Successful);
        }

        [Fact]
        public async void GetLostDogsFailsForReporError()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterDogRepo.Setup(o => o.GetShelterDogs(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<List<ShelterDog>, int>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.GetShelterDogs(1, 0, 0)).Successful);
        }

        [Fact]
        public async void GetLostDogsDetailsSuccessfulForExistingDog()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterDogRepo.Setup(o => o.GetShelterDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<ShelterDog>()));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.GetShelterDogDetails(1)).Successful);
        }

        [Fact]
        public async void GetLostDogsDetailsFailsForNotExistingDog()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            shelterDogRepo.Setup(o => o.GetShelterDogDetails(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<ShelterDog>() { Successful = false }));
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.GetShelterDogDetails(1)).Successful);
        }

        [Fact]
        public async void AddLostDogSuccessfulForValidPostDogWithImage()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();

            using var memoryStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            var dogDto = new UploadShelterDogDto();
            var dog = mapper.Map<ShelterDog>(dogDto);
            shelterDogRepo.Setup(o => o.AddShelterDog(It.IsAny<ShelterDog>())).Returns((ShelterDog d) => Task.FromResult(new RepositoryResponse<ShelterDog>() { Data = d }));
            security.Setup(s => s.IsPictureValid(It.IsAny<IFormFile>())).Returns((IFormFile f) => new ServiceResponse());
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.True((await service.AddShelterDog(dogDto, picture)).Successful);
        }

        [Fact]
        public async void AddLostDogFailsForValidPostDogAndInvalidImage()
        {
            var shelterRepo = new Mock<IShelterRepository>();
            var shelterDogRepo = new Mock<IShelterDogRepository>();
            var security = new Mock<ISecurityService>();
            var account = new Mock<IAccountService>();
            var picture = new FormFile(null, 0, 0, "name", "filename");
            var dogDto = new UploadShelterDogDto();
            var dog = mapper.Map<ShelterDog>(dogDto);
            shelterDogRepo.Setup(o => o.AddShelterDog(dog)).Returns(Task.FromResult(new RepositoryResponse<ShelterDog>() { Data = dog }));
            security.Setup(s => s.IsPictureValid(It.IsAny<IFormFile>())).Returns((IFormFile f) => new ServiceResponse() { Successful = false });
            var service = new ShelterService(shelterRepo.Object, shelterDogRepo.Object, account.Object, security.Object, mapper, logger);

            Assert.False((await service.AddShelterDog(dogDto, null)).Successful);
        }
    }
}
