using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace SportHub.Services
{
    public class ImageService : IImageService
    {
        private readonly string _containerName;
        private BlobContainerClient _blobContainerClient;
        private readonly List<string> allowedExtensions = new List<string>() {".png", ".jpg", ".jpeg", ".gif"};

        public ImageService(BlobContainerClient blobContainerClient)
        {
            _blobContainerClient = blobContainerClient;
            _containerName = _blobContainerClient.Name;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            try
            {
                string extension = Path.GetExtension(image.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    return null;
                }
                string imageName = Guid.NewGuid().ToString() + extension;
                BlobClient blobClient = _blobContainerClient.GetBlobClient(imageName);
                await using (Stream? data = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(data);
                }
                return imageName;
            }
            catch
            {
                return null;
            }
        }

        //return list of strings with all filenames
        public async Task< List<string> > GetImagesListAsync()
        {
            var listOfFileNames = new List<string>();
            await foreach (var blobItem in _blobContainerClient.GetBlobsAsync())
            {
                listOfFileNames.Add(blobItem.Name);
            }
            return listOfFileNames;
        }

        //return absolute link to image by filename (with extension). null if doesn't exists
        public async Task<string> GetImageLinkByNameAsync(string imageName)
        {
            BlobClient fileClient = _blobContainerClient.GetBlobClient(imageName);
            if (!(await fileClient.ExistsAsync()))
            {
                return null; 
            }
            //if blob was found, generate SAS link
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobName = imageName,
                BlobContainerName = _containerName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            Uri sasUri = fileClient.GenerateSasUri(sasBuilder);

            return sasUri.ToString();  
        }

        public string GetImageLinkByName(string imageName)
        {
            BlobClient fileClient = _blobContainerClient.GetBlobClient(imageName);
            if (!(fileClient.Exists()))
            {
                return null;
            }
            //if blob was found, generate SAS link
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobName = imageName,
                BlobContainerName = _containerName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            Uri sasUri = fileClient.GenerateSasUri(sasBuilder);

            return sasUri.ToString();
        }
    }
}
