using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jobtech.OpenPlatforms.GigPlatformApi.FileStore.Managers
{
    public interface IFileManager
    {
        Task<Uri> SaveAsync(IFormFile file);
    }
}