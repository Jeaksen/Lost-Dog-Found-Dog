using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using System;
using System.Linq;
using Xunit;

namespace Backend.Tests.Accounts
{
    [Collection("Database collection")]
    public class AccountServiceTests
    {

        private static Random random = new Random();
        private readonly DatabaseFixture databaseAuthFixture;
        private const string chars = "abcdefghijklmnoprstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int nameLenght = 40;


        public AccountServiceTests(DatabaseFixture databaseAuthFixture)
        {
            this.databaseAuthFixture = databaseAuthFixture;
        }

        private Account GetValidAccount()
        {
            var uname = new string(Enumerable.Range(1, nameLenght).Select(_ => chars[random.Next(chars.Length)]).ToArray());
            return new Account() { 
                UserName = uname, 
                Email = $"{uname}@gmail.com", 
                PhoneNumber = "222333444" 
            };
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void SucceedForValidUserAndPassword(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);
        }

        [InlineData("Short66")]
        [Theory]
        public async void FailForForPasswordShorterThanEightDigits(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("short666^")]
        [Theory]
        public async void FailForForPasswordWithoutCapitalLetters(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("Shortttt")]
        [Theory]
        public async void FailForForPasswordWithoutDigits(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("SHORT666")]
        [Theory]
        public async void FailForForPasswordWithoutSmallLetters(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void FailForCreatingUsersWithTheSameName(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            Assert.True((await databaseAuthFixture.AccountService.AddAccount(addAccountDto)).Successful);
            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);

            Assert.False(result.Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void LoginSuccefullForExistingUserAndValidPassword(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };

            Assert.True((await databaseAuthFixture.AccountService.AddAccount(addAccountDto)).Successful);
            var loginDto = new LoginDto()
            {
                UserName = account.UserName,
                Password = password
            };

            Assert.True((await databaseAuthFixture.AccountService.Authenticate(loginDto)).Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void FindSuccessForExistingUser(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };

            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            Assert.True((await databaseAuthFixture.AccountService.GetAccountById(result.Data.Id)).Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void UsersListNotEmptyForDatabaseWithUsers(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };

            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            Assert.True((await databaseAuthFixture.AccountService.GetAllAccountsForRole(AccountRoles.Regular)).Data.Count > 0);
        }

        [Fact]
        public async void UpdateExistingAccountSuccessful()
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "Test123456"
            };

            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var updateAccountDto = new UpdateAccountDto()
            {
                Name = account.UserName + "a",
                Email = "ab" + account.Email,
                PhoneNumber = account.PhoneNumber
            };
            Assert.True((await databaseAuthFixture.AccountService.UpdateAccount(updateAccountDto, result.Data.Id)).Successful);
        }

        [Fact]
        public async void UpdateNonExistingAccountFails()
        {
            var account = GetValidAccount();
            var updateAccountDto = new UpdateAccountDto()
            {
                Name = account.UserName + "a",
                Email = "ab" + account.Email,
                PhoneNumber = account.PhoneNumber
            };
            Assert.False((await databaseAuthFixture.AccountService.UpdateAccount(updateAccountDto, -1)).Successful);
        }

        [Fact]
        public async void UpdateExistingAccountForOccupiedUserNameFails()
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "Test123456"
            };

            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var existingUsername = account.UserName; 
            account = GetValidAccount();
            addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "Test123456"
            };

            result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var updateAccountDto = new UpdateAccountDto()
            {
                Name = existingUsername,
                Email = "ac" + account.Email,
                PhoneNumber = account.PhoneNumber
            };
            Assert.False((await databaseAuthFixture.AccountService.UpdateAccount(updateAccountDto, result.Data.Id)).Successful);
        }

        [Fact]
        public async void UpdateExistingAccountForOccupiedEmailFails()
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "Test123456"
            };

            var result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var existingEmail = account.Email;
            account = GetValidAccount();
            addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "Test123456"
            };

            result = await databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var updateAccountDto = new UpdateAccountDto()
            {
                Name = "a" + account.UserName,
                Email = existingEmail,
                PhoneNumber = account.PhoneNumber
            };
            Assert.False((await databaseAuthFixture.AccountService.UpdateAccount(updateAccountDto, result.Data.Id)).Successful);
        }
    }
}
