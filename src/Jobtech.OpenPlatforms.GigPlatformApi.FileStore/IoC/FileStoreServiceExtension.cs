using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Config;
using Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobtech.OpenPlatforms.GigPlatformApi.FileStore.IoC
{
    public static class FileStoreServiceExtension
    {
        public static IServiceCollection AddFileStore(this IServiceCollection collection, IConfiguration configuration)
        {
            var fileStoreSection = configuration.GetSection("FileStore");
            collection.Configure<FileStoreConfig>(c =>
            {
                c.AccessKey = fileStoreSection.GetValue<string>("AccessKey");
                c.SecretKey = fileStoreSection.GetValue<string>("SecretKey");
                c.StorageEndpoint = fileStoreSection.GetValue<string>("StorageEndpoint");
                c.BucketName = fileStoreSection.GetValue<string>("BucketName");
            });

            collection.AddTransient<IFileManager, FileManager>();

            return collection;
        }
    }
}
