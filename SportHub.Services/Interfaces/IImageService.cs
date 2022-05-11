using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SportHub.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile formFile);
        public Task<List<string>> GetImagesListAsync();
        public Task<string> GetImageLinkByNameAsync(string filename);
        public string GetImageLinkByName(string filename);
    }
}
