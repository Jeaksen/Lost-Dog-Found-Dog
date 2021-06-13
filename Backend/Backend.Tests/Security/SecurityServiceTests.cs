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
        [InlineData(63)]
        [InlineData(1023)]
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
        [InlineData(1024)]
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
        [InlineData("application/json", 1024)]
        public void IsPictureValidFailsForFileWithInvalidMimeType(string mimeType, int fileSize)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, fileSize).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", "filename.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            Assert.False(securityService.IsPictureValid(picture).Successful);
        }

        [Theory]
        [InlineData("image/png", "file.jpg", 1024)]
        [InlineData("image/jpeg", "file.png", 1024)]
        public void IsPictureValidFailsForFileWithInvalidExtension(string mimeType, string filename, int fileSize)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, fileSize).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", filename)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            Assert.False(securityService.IsPictureValid(picture).Successful);
        }

        [Theory]
        [InlineData("image/png", "file.png", 1024)]
        [InlineData("image/jpeg", "file.jpeg", 1024)]
        [InlineData("image/jpeg", "file.jpg", 1024)]
        public void IsPictureValidSuccessfulForFileWithValidExtension(string mimeType, string filename, int fileSize)
        {
            using var memoryStream = new MemoryStream(Enumerable.Repeat((byte)1, fileSize).ToArray());
            var picture = new FormFile(memoryStream, 0, memoryStream.Length, "name", filename)
            {
                Headers = new HeaderDictionary(),
                ContentType = mimeType
            };

            Assert.True(securityService.IsPictureValid(picture).Successful);
        }
    }
}
