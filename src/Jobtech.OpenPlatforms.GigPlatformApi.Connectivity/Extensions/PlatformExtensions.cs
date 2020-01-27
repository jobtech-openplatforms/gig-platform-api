using System;
using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Extensions
{
    public static class PlatformExtensions
    {
        public static Platform Activate(this Platform platform) => new Platform { Id = platform.Id, PlatformToken = platform.PlatformToken, ExportDataUri = platform.ExportDataUri, LastUpdate = DateTime.UtcNow, Published = true };

        public static Platform Deactivate(this Platform platform) => new Platform { Id = platform.Id, PlatformToken = platform.PlatformToken, ExportDataUri = platform.ExportDataUri, LastUpdate = DateTime.UtcNow, Published = false };
    }
}