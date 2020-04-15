using System;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities
{
    public class Platform
    {
        public Guid Id { get; set; }
        public string PlatformToken { get; set; }
        public string ExportDataUri { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Published { get; set; }

        public static Platform Create() => new Platform { Id = Guid.NewGuid(), PlatformToken = Guid.NewGuid().ToString(), LastUpdate = DateTime.UtcNow };
        public static Platform Create(Guid platformId) => new Platform { Id = platformId, PlatformToken = Guid.NewGuid().ToString(), LastUpdate = DateTime.UtcNow };
        public static Platform Create(Guid platformId, string exportDataUri) => new Platform { Id = platformId, PlatformToken = Guid.NewGuid().ToString(), ExportDataUri = exportDataUri, LastUpdate = DateTime.UtcNow };

        public Platform RegisteredWithId(Guid id)
        {
            this.Id = id;
            return this;
        }
    }
}
