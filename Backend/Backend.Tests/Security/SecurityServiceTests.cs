using Backend.Services.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Backend.Tests.Security
{
    public class SecurityServiceTests
    {
        readonly ISecurityService securityService;

        public SecurityServiceTests()
        {
            securityService = new SecurityService();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(64)]
        public void IsPictureValidFailsForFileWithTooSmallSize(int size)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, size).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            Assert.False(securityService.IsPictureValid(picture).Successful);
        }

        [Theory]
        [InlineData(65)]
        public void IsPictureValidSuccessfulForFileWithBigEnoughSize(int size)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, size).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            Assert.True(securityService.IsPictureValid(picture).Successful);
        }

        [Theory]
        [InlineData("application/json")]
        public void IsPictureValidFailsForFileWithInvalidMimeType(string mimeType)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, 150).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            Assert.False(securityService.IsPictureValid(picture).Successful);
        }

        [Theory]
        [InlineData("image/png", "file.jpg")]
        [InlineData("image/jpeg", "file.png")]
        public void IsPictureValidFailsForFileWithInvalidExtension(string mimeType, string filename)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, 150).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", filename)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            Assert.False(securityService.IsPictureValid(picture).Successful);
        }

        [Theory]
        [InlineData("image/png", "file.png")]
        [InlineData("image/jpeg", "file.jpeg")]
        [InlineData("image/jpeg", "file.jpg")]
        public void IsPictureValidSuccessfulForFileWithValidExtension(string mimeType, string filename)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, 150).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", filename)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            Assert.True(securityService.IsPictureValid(picture).Successful);
        }
    }
}
