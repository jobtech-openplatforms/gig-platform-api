using Jobtech.OpenPlatforms.GigPlatformApi.Core.Entities;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects
{
    public class ConnectionId : StringIdentity<Connection>
    {
        public static implicit operator ConnectionId(string value) => new ConnectionId { Value= value };
    }
}