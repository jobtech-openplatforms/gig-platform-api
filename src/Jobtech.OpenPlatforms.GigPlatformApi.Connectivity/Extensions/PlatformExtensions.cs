using AF.GigPlatform.Core.Entities;
using System;

namespace AF.GigPlatform.Connectivity.Extensions
{
    public static class PlatformExtensions
    {
        public static Platform Activate(this Platform platform) => new Platform { Id = platform.Id, PlatformToken = platform.PlatformToken, ExportDataUri = platform.ExportDataUri, LastUpdate = DateTime.UtcNow, Published = true };

        public static Platform Deactivate(this Platform platform) => new Platform { Id = platform.Id, PlatformToken = platform.PlatformToken, ExportDataUri = platform.ExportDataUri, LastUpdate = DateTime.UtcNow, Published = false };
    }
}