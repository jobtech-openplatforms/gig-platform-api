using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Config;
using Microsoft.Extensions.Options;

namespace Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Services
{
    public class FileStorageConfigService : IFileStorageConfigService
    {
        public FileStorageConfigService(IOptions<FileStoreConfig> config)
        {
            ConnectionString = config.Value.AzureBlobStorageConnection;
        }

        public string ConnectionString { get; }
    }
}