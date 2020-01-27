using AF.GigPlatform.Core.Entities;

namespace AF.GigPlatform.Core.ValueObjects
{
    public class ConnectionId : StringIdentity<Connection>
    {
        public static implicit operator ConnectionId(string value) => new ConnectionId { Value= value };
    }
}