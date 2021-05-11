using AutoMapper;
using Backend.DataAccess.Shelters;
using Backend.DTOs.Authentication;
using Backend.DTOs.Shelters;
using Backend.Models.Response;
using Backend.Models.Shelters;
using Backend.Services.Authentication;
using Backend.Services.Shelters;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async void AddShelterSuccessfulForSuccessfulRepoAndAccountServiceResponse()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter()}));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() {Data = new GetAccountDto() }));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.True((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterFailsForFailedRepoResponse()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false}));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.False((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterFailsForFailedAccountServiceResponseAndSuccessfulShelterDelete()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            repo.Setup(r => r.DeleteShelterWithoutDogs(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() { Successful = false }));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.False((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void AddShelterFailsForFailedAccountServiceResponseAndFailedShelterDelete()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.AddShelter(It.IsAny<Shelter>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter() }));
            repo.Setup(r => r.DeleteShelterWithoutDogs(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse() { Successful = false}));
            account.Setup(s => s.AddShelterAccount(It.IsAny<Shelter>())).Returns(Task.FromResult(new ServiceResponse<GetAccountDto>() { Successful = false }));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.False((await service.AddShelter(new ShelterDto())).Successful);
        }

        [Fact]
        public async void GetShelterSuccessfulForSuccessRepoResponse()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.GetShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Data = new Shelter () }));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.True((await service.GetShelter(1)).Successful);
        }

        [Fact]
        public async void GetShelterFailsForFailedRepoResponse()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.GetShelter(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse<Shelter>() { Successful = false }));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.False((await service.GetShelter(-1)).Successful);
        }

        [Fact]
        public async void DeleteShelterSuccessfulForSuccessRepoResponse()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.DeleteShelterWithoutDogs(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse()));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.True((await service.DeleteShelter(1)).Successful);
        }

        [Fact]
        public async void DeleteShelterFailsForFailedRepoResponse()
        {
            var repo = new Mock<IShelterRepository>();
            var account = new Mock<IAccountService>();
            repo.Setup(r => r.DeleteShelterWithoutDogs(It.IsAny<int>())).Returns(Task.FromResult(new RepositoryResponse() { Successful = false }));
            var service = new ShelterService(repo.Object, account.Object, mapper, logger);

            Assert.False((await service.DeleteShelter(-1)).Successful);
        }
    }
}
