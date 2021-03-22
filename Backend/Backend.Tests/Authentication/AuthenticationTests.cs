using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using System;
using System.Linq;
using Xunit;

namespace Backend.Tests
{
    [Collection("Database collection")]
    public class AuthenticationTests
    {

        private static Random random = new Random();
        private readonly DatabaseFixture _databaseAuthFixture;
        private const string chars = "abcdefghijklmnoprstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int nameLenght = 40;


        public AuthenticationTests(DatabaseFixture databaseAuthFixture)
        {
            _databaseAuthFixture = databaseAuthFixture;
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
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);
        }

        [InlineData("Short66")]
        [Theory]
        public async void FailForForPasswordShorterThanEightDigits(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("short666^")]
        [Theory]
        public async void FailForForPasswordWithoutCapitalLetters(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("Shortttt")]
        [Theory]
        public async void FailForForPasswordWithoutDigits(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("SHORT666")]
        [Theory]
        public async void FailForForPasswordWithoutSmallLetters(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void FailForCreatingUsersWithTheSameName(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);

            Assert.False(result.Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void LoginSuccefullForExistingUserAndValidPassword(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };

            await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);
            var loginDto = new LoginDto()
            {
                UserName = account.UserName,
                Password = password
            };

            Assert.True((await _databaseAuthFixture.AccountService.Authenticate(loginDto)).Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void FindSuccessForExistingUser(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };

            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);

            Assert.True((await _databaseAuthFixture.AccountService.GetAccountById(result.Data.Id)).Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void UsersListNotEmptyForDatabaseWithUsers(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };

            var result = await _databaseAuthFixture.AccountService.AddAccount(addAccountDto);

            Assert.True((await _databaseAuthFixture.AccountService.GetAllAccountsForRole(AccountRoles.User)).Data.Count > 0);
        }
    }
}
