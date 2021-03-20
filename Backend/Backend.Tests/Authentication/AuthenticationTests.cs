using Backend.Models.Authentication;
using Backend.Services.AuthenticationService;
using System;
using System.Linq;
using Xunit;

namespace Backend.Tests
{
    [Collection("Database Auth collection")]
    public class AuthenticationTests
    {

        private static Random random = new Random();
        private readonly DatabaseAuthFixture databaseAuthFixture;
        private const string chars = "abcdefghijklmnoprstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int nameLenght = 40;


        public AuthenticationTests(DatabaseAuthFixture databaseAuthFixture)
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

        [InlineData("LongSafePas6^")]
        [Theory]
        public async void SucceedForValidUserAndPassword(string password)
        {

            var user = await databaseAuthFixture.UserManager.CreateAsync(GetValidAccount(), password);
            Assert.True(user.Succeeded);
        }

        [InlineData("short")]
        [Theory]
        public async void FailForForPasswordShorterThanSixDigits(string password)
        {
            var user = await databaseAuthFixture.UserManager.CreateAsync(GetValidAccount(), password);
            Assert.False(user.Succeeded);
        }
    }
}
