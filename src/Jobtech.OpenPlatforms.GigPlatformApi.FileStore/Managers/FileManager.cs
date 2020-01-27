using System;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers
{
    public class FileManager : IFileManager
    {

        private readonly IFileStorageConfigService _configuration;

        public FileManager(IFileStorageConfigService configuration)
        {
            _configuration = configuration;
        }

        public async Task<Uri> SaveAsync(IFormFile file)
        {

            if (CloudStorageAccount.TryParse(_configuration.ConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference("logo-storage");
                BlobContainerPermissions permissions = await container.GetPermissionsAsync();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                await container.SetPermissionsAsync(permissions);

                await container.CreateIfNotExistsAsync();

                if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                     !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                     !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                     !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                     !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                     !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("Incorrect file type");
                }

                var fileExtension = "";

                switch (file.ContentType)
                {
                    case "image/jpg":
                    case "image/jpeg":
                    case "image/pjpeg":
                        fileExtension = ".jpg";
                        break;
                    case "image/x-png":
                    case "image/png":
                        fileExtension = ".png";
                        break;
                    case "image/gif":
                        fileExtension = ".gif";
                        break;
                    default:
                        throw new Exception("Incorrect file type");
                }

                // Don't rely on or trust the FileName property without validation. The FileName property should only be used for display purposes.
                var picBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + fileExtension);
                picBlob.Properties.ContentType = file.ContentType;

                await picBlob.UploadFromStreamAsync(file.OpenReadStream());

                return picBlob.Uri;
            }

            throw new Exception("Unable to save the file");
        }
    }
}
