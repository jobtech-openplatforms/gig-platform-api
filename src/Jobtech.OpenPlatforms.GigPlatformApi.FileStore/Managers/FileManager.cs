using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers
{
    public class FileManager : IFileManager
    {

        private readonly FileStoreConfig _configuration;

        public FileManager(IOptions<FileStoreConfig> configuration)
        {
            _configuration = configuration.Value;
        }

        /// <summary>
        /// Uploads a file to storage.
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="name">The file name</param>
        /// <param name="path">The prefix to the file name (i.e subdirectories).</param>
        /// <returns></returns>
        public async Task<Uri> UploadFileAsync(IFormFile file, string name, string path, CancellationToken cancellationToken = default)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` environment variable.
                ServiceURL = _configuration.StorageEndpoint,
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };
            var amazonS3Client = new AmazonS3Client(_configuration.AccessKey, _configuration.SecretKey, config);

            var key = $"{path}/{name}";

            using var stream = file.OpenReadStream();
            stream.Position = 0;

            var result = await amazonS3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest { 
                BucketName = _configuration.BucketName, 
                Key = key, 
                InputStream = stream,
                ContentType = file.ContentType
            }, cancellationToken);

            var url = $"{_configuration.StorageEndpoint}/{_configuration.BucketName}/{key}";

            return new Uri(url);
        }
    }
}
