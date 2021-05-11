using Backend.DataAccess.Shelters;
using Backend.Models.Shelters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Shelters
{
    [Collection("Database collection")]
    public class ShelterRepositoryTests
    {

        private readonly IShelterRepository shelterRepository;

        public ShelterRepositoryTests(DatabaseFixture databaseAuthFixture)
        {
            shelterRepository = databaseAuthFixture.ShelterRepository;
        }

        static int counter = 1;
        static Shelter GetValidShelter()
        {
            var shelter = new Shelter()
            {
                IsApproved = true,
                Name = $"VeryNiceShelter{counter}",
                PhoneNumber = $"11111111{counter}",
                Email = $"shelter{counter}@gmail.com",
                Address = new Address
                {
                    City = "Gdańsk",
                    PostCode = "12-345",
                    Street = "Bursztynowa",
                    BuildingNumber = "123"
                }
            };
            counter++;
            return shelter;
        }

        [Fact]
        public async void AddShelterSuccessfulForShelter()
        {
            var addShelter = GetValidShelter();
            var result = await shelterRepository.AddShelter(addShelter);
            Assert.True(result.Successful);
        }

        [Fact]
        public async void AddShelterSuccessfulForShelterWithAddressWithoutAdditionalLine()
        {
            var addShelter = GetValidShelter();
            var result = await shelterRepository.AddShelter(addShelter);
            Assert.True(result.Successful);
        }

        [Fact]
        public async void AddShelterFailsForMissingAddressPart()
        {
            var addShelter = new Shelter()
            {
                IsApproved = true,
                Name = "VeryNiceShelter4",
                PhoneNumber = "111111114",
                Email = "shelter4@gmail.com",
                Address = new Address
                {
                    City = "Gdańsk",
                    PostCode = "12-345",
                    Street = "Bursztynowa",
                    BuildingNumber = "123"
                }
            };

            var buildingNumber = addShelter.Address.BuildingNumber;
            addShelter.Address.BuildingNumber = null;
            var result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);

            addShelter.Address.BuildingNumber = buildingNumber;
            var street = addShelter.Address.Street;
            addShelter.Address.Street = null;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);

            addShelter.Address.Street = street;
            var postCode = addShelter.Address.PostCode;
            addShelter.Address.PostCode = null;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);

            addShelter.Address.PostCode = postCode;
            addShelter.Address.City = null;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void AddShelterFailsForMissingShelterPart()
        {
            var addShelter = GetValidShelter();

            var address = addShelter.Address;
            addShelter.Address = null;
            var result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);

            addShelter.Address = address;
            var email = addShelter.Email;
            addShelter.Email = null;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);

            addShelter.Email = email;
            var phoneNumber = addShelter.PhoneNumber;
            addShelter.PhoneNumber = null;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);

            addShelter.PhoneNumber = phoneNumber;
            addShelter.PhoneNumber = null;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void AddShelterFailsForDuplicateEmail()
        {
            var addShelter = GetValidShelter();
            var result = await shelterRepository.AddShelter(addShelter);
            Assert.True(result.Successful);
            addShelter.Name = "justsomerandomname";
            addShelter.Id = 0;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void AddShelterFailsForDuplicateName()
        {
            var addShelter = GetValidShelter();
            var result = await shelterRepository.AddShelter(addShelter);
            Assert.True(result.Successful);
            addShelter.Name = "justsomerandomemail@gmail.com";
            addShelter.Id = 0;
            result = await shelterRepository.AddShelter(addShelter);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void GetShelterFailsForNotExistingShelter()
        {
            var result = await shelterRepository.GetShelter(-1);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void GetShelterSuccessfulForExistingShelter()
        {
            var addShelter = GetValidShelter();
            var addResult = await shelterRepository.AddShelter(addShelter);
            Assert.True(addResult.Successful);

            var result = await shelterRepository.GetShelter(addResult.Data.Id);
            Assert.True(result.Successful);
        }

        [Fact]
        public async void DeleteShelterWithoutDogsFailsForNotExistingShelter()
        {
            var result = await shelterRepository.DeleteShelterWithoutDogs(-1);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void DeleteShelterWithoutDogsSuccessfulForExistingShelter()
        {
            var addShelter = GetValidShelter();
            var addResult = await shelterRepository.AddShelter(addShelter);
            Assert.True(addResult.Successful);

            var result = await shelterRepository.DeleteShelterWithoutDogs(addResult.Data.Id);
            Assert.True(result.Successful);
        }

    }
}
