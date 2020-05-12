using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers
{
    public interface IFileManager
    {
        Task<Uri> UploadFileAsync(IFormFile file, string name, string path, CancellationToken cancellationToken = default);
    }
}