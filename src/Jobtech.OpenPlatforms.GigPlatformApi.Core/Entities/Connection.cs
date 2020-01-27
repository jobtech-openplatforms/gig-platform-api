
using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities
{
    public class Connection
    {
        public string Id { get; set; }
        public string PlatformId { get; set; }
        public string UserId { get; set; }
        public string PlatformToken { get; set; }
        public string UserAccessToken { get; set; }
        public IsoDateTimeUtc LastUpdatedUtc { get; set; }
    }
}