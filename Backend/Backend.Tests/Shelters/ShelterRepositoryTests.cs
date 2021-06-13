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
        public async void AddShelterSuccessfulForValidShelter()
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
        public async void ApproveShelterSuccessfulForExistingNotApprovedShelter()
        {
            var addShelter = GetValidShelter();
            addShelter.IsApproved = false;
            var resultAdd = await shelterRepository.AddShelter(addShelter);
            Assert.True(resultAdd.Successful);
            var resultAprove = await shelterRepository.ApproveShelter(resultAdd.Data.Id);
            Assert.True(resultAprove.Successful);
        }

        [Fact]
        public async void ApproveShelterFailsForNotExistingShelter()
        {
            var resultAprove = await shelterRepository.ApproveShelter(-1);
            Assert.False(resultAprove.Successful);
        }

        [Fact]
        public async void ApproveShelterFailsForAlreadyApprovedShelter()
        {
            var addShelter = GetValidShelter();
            addShelter.IsApproved = false;
            var resultAdd = await shelterRepository.AddShelter(addShelter);
            Assert.True(resultAdd.Successful);
            var resultAprove = await shelterRepository.ApproveShelter(resultAdd.Data.Id);
            Assert.True(resultAprove.Successful);
            resultAprove = await shelterRepository.ApproveShelter(resultAdd.Data.Id);
            Assert.False(resultAprove.Successful);
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
            var result = await shelterRepository.DeleteShelter(-1);
            Assert.False(result.Successful);
        }

        [Fact]
        public async void DeleteShelterWithoutDogsSuccessfulForExistingShelter()
        {
            var addShelter = GetValidShelter();
            var addResult = await shelterRepository.AddShelter(addShelter);
            Assert.True(addResult.Successful);

            var result = await shelterRepository.DeleteShelter(addResult.Data.Id);
            Assert.True(result.Successful);
        }

        [Fact]
        public async void GetSheltersReturnsDogsForNotEmptyDatabase()
        {
            var addShelter = GetValidShelter();
            var addResult = await shelterRepository.AddShelter(addShelter);
            Assert.True(addResult.Successful);

            var result = await shelterRepository.GetShelters(null, null, 0, 5);
            Assert.True(result.Successful);
            Assert.True(result.Data.Count > 0);
        }

        [Theory]
        [InlineData(15, 5)]
        public async void GetSheltersPagingSplitsDogsCorrectly(int minDogsCount, int pageSize)
        {
            for (int i = 0; i < minDogsCount; i++)
            {
                var addShelter = GetValidShelter();
                var addResult = await shelterRepository.AddShelter(addShelter);
                Assert.True(addResult.Successful);
            }
            var result = await shelterRepository.GetShelters(null, null, 0, pageSize);

            Assert.True(result.Successful);
            Assert.True(Math.Ceiling(minDogsCount / (double)pageSize) <= result.Metadata);
            Assert.Equal(pageSize, result.Data.Count);
        }

        [Fact]
        public async void GetSheltersNameSortingWorksCorrectly()
        {

            var addShelter = GetValidShelter();
            var addShelter2 = GetValidShelter();
            var name1 = addShelter.Name + "B";
            var name2 = addShelter.Name + "A";
            addShelter.Name = name1;
            addShelter2.Name = name2;
            var addResult1 = await shelterRepository.AddShelter(addShelter);
            var addResult2 = await shelterRepository.AddShelter(addShelter2);
            Assert.True(addResult1.Successful);
            Assert.True(addResult2.Successful);

            var result = await shelterRepository.GetShelters(null, null, 0, 500);
            Assert.True(result.Successful);

            int shelter1Index = result.Data.FindIndex(s => s.Id == addResult1.Data.Id);
            int shelter2Index = result.Data.FindIndex(s => s.Id == addResult2.Data.Id);

            Assert.True(shelter2Index < shelter1Index);

            result = await shelterRepository.GetShelters(null, "name,asc", 0, 500);
            Assert.True(result.Successful);

            shelter1Index = result.Data.FindIndex(s => s.Id == addResult1.Data.Id);
            shelter2Index = result.Data.FindIndex(s => s.Id == addResult2.Data.Id);

            Assert.True(shelter2Index < shelter1Index);

            result = await shelterRepository.GetShelters(null, "name,desc", 0, 500);
            Assert.True(result.Successful);

            shelter1Index = result.Data.FindIndex(s => s.Id == addResult1.Data.Id);
            shelter2Index = result.Data.FindIndex(s => s.Id == addResult2.Data.Id);

            Assert.True(shelter2Index > shelter1Index);
        }

    }
}
