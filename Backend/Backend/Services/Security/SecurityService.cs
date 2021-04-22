using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Services.Security
{
    public class SecurityService : ISecurityService
    {
        private List<string> AllowedPictureMimeTypes = new List<string>() { "image/png", "image/jpeg" };
        private Dictionary<string, List<string>> ExtensionsForMimeType = new Dictionary<string, List<string>>()
        {
            { "image/png", new List<string>() { "png"} },
            { "image/jpeg", new List<string>() { "jpg", "jpeg" } }
        };
        private int MinimalPictureSize = 64;

        public (bool Successful, string Message) IsPictureValid(IFormFile picture)
        {
            if (picture.Length < MinimalPictureSize)
                return (false, $"Picture has size smaller than {MinimalPictureSize}!");

            var typeIndex = AllowedPictureMimeTypes.FindIndex(s => s == picture.ContentType);
            if (!AllowedPictureMimeTypes.Contains(picture.ContentType))
                return (false, $"Type {picture.ContentType} is not an allowed typ for pictures!");

            var ext = picture.FileName.Split('.').Last();

            if (!ExtensionsForMimeType[picture.ContentType].Contains(ext))
                return (false, $"Picture has an invalid extension: {MinimalPictureSize}, expected one of [ {string.Join(", ", ExtensionsForMimeType[picture.ContentType])}]!");

            return (true, "picture valid");
        }
    }
}
