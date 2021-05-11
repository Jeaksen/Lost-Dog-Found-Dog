using Backend.DTOs.Authentication;
using Backend.Models.Authentication;
using Backend.Services.Authentication;
using System;
using System.Linq;
using Xunit;

namespace Backend.Tests.Accounts
{
    [Collection("Database collection")]
    public class AccountServiceTests
    {

        private static readonly Random random = new();
        private const string chars = "abcdefghijklmnoprstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int nameLenght = 40;
        public IAccountService accountService;

        public AccountServiceTests(DatabaseFixture databaseAuthFixture)
        {
            accountService = databaseAuthFixture.AccountService;
        }

        private Account GetValidAccount()
        {
            var uname = new string(Enumerable.Range(1, nameLenght).Select(_ => chars[random.Next(chars.Length)]).ToArray());
            return new Account()
            {
                UserName = uname,
                Email = $"{uname}@gmail.com",
                PhoneNumber = "222333444"
            };
        }

        [Theory]
        [InlineData("LongSafePas6", AccountRoles.Regular)]
        [InlineData("LongSafePas6", AccountRoles.Shelter)]
        [InlineData("LongSafePas6", AccountRoles.Admin)]
        public async void CreateAccountSucceedForValidUserAndPassword(string password, string role)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password,
                AccountRole = role
            };
            var result = await accountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);
        }

        [InlineData("Short66")]
        [Theory]
        public async void CreateAccountFailForForPasswordShorterThanEightDigits(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await accountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("short666^")]
        [Theory]
        public async void CreateAccountFailForForPasswordWithoutCapitalLetters(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await accountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("Shortttt")]
        [Theory]
        public async void CreateAccountFailForForPasswordWithoutDigits(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await accountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("SHORT666")]
        [Theory]
        public async void CreateAccountFailForForPasswordWithoutSmallLetters(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            var result = await accountService.AddAccount(addAccountDto);
            Assert.False(result.Successful);
        }

        [InlineData("LongSafePas6")]
        [Theory]
        public async void CreateAccountFailForCreatingUsersWithTheSameName(string password)
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = password
            };
            Assert.True((await accountService.AddAccount(addAccountDto)).Successful);
            var result = await accountService.AddAccount(addAccountDto);

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

            Assert.True((await accountService.AddAccount(addAccountDto)).Successful);
            var loginDto = new LoginDto()
            {
                UserName = account.UserName,
                Password = password
            };

            Assert.True((await accountService.Authenticate(loginDto)).Successful);
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

            var result = await accountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            Assert.True((await accountService.GetAccountById(result.Data.Id)).Successful);
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

            var result = await accountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            Assert.True((await accountService.GetAllAccountsForRole(AccountRoles.Regular)).Data.Count > 0);
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

            var result = await accountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var updateAccountDto = new UpdateAccountDto()
            {
                Name = account.UserName + "a",
                Email = "ab" + account.Email,
                PhoneNumber = account.PhoneNumber
            };
            Assert.True((await accountService.UpdateAccount(updateAccountDto, result.Data.Id)).Successful);
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
            Assert.False((await accountService.UpdateAccount(updateAccountDto, -1)).Successful);
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

            var result = await accountService.AddAccount(addAccountDto);
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

            result = await accountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var updateAccountDto = new UpdateAccountDto()
            {
                Name = existingUsername,
                Email = "ac" + account.Email,
                PhoneNumber = account.PhoneNumber
            };
            Assert.False((await accountService.UpdateAccount(updateAccountDto, result.Data.Id)).Successful);
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

            var result = await accountService.AddAccount(addAccountDto);
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

            result = await accountService.AddAccount(addAccountDto);
            Assert.True(result.Successful);

            var updateAccountDto = new UpdateAccountDto()
            {
                Name = "a" + account.UserName,
                Email = existingEmail,
                PhoneNumber = account.PhoneNumber
            };
            Assert.False((await accountService.UpdateAccount(updateAccountDto, result.Data.Id)).Successful);
        }

        [Fact]
        public async void DeleteAccountByIdSuccessForExistingAccount()
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName + "a",
                Email = "ab" + account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "LongSafePas6"
            };
            var addResult = await accountService.AddAccount(addAccountDto);
            Assert.True(addResult.Successful);

            Assert.True((await accountService.DeleteAccount(id: addResult.Data.Id)).Successful);
        }

        [Fact]
        public async void DeleteAccountByIdFailsForNotExistingAccount()
        {
            Assert.False((await accountService.DeleteAccount(id: -1)).Successful);
        }

        [Fact]
        public async void DeleteAccountByUsernameSuccessForExistingAccount()
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName + "a",
                Email = "ab" + account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "LongSafePas6"
            };
            var addResult = await accountService.AddAccount(addAccountDto);
            Assert.True(addResult.Successful);

            Assert.True((await accountService.DeleteAccount(username: addResult.Data.Name)).Successful);
        }

        [Fact]
        public async void DeleteAccountByUsernameFailsForNotExistingAccount()
        {
            var account = GetValidAccount();

            Assert.False((await accountService.DeleteAccount(username: account.UserName)).Successful);
        }

        [Fact]
        public async void DeleteAccountByEmailSuccessForExistingAccount()
        {
            var account = GetValidAccount();
            var addAccountDto = new AddAccountDto()
            {
                Name = account.UserName + "a",
                Email = "ab" + account.Email,
                PhoneNumber = account.PhoneNumber,
                Password = "LongSafePas6"
            };
            var addResult = await accountService.AddAccount(addAccountDto);
            Assert.True(addResult.Successful);

            Assert.True((await accountService.DeleteAccount(email: addResult.Data.Email)).Successful);
        }

        [Fact]
        public async void DeleteAccountByEmaildFailsForNotExistingAccount()
        {
            var account = GetValidAccount();

            Assert.False((await accountService.DeleteAccount(email: account.Email)).Successful);
        }
    }
}
