using Jobtech.OpenPlatforms.GigPlatformApi.Core.ValueObjects;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// API model to request a sync of data between a User and a Platform
    ///</summary>
    public class ConnectionSyncRequest
    {
        public ConnectionId ConnectionId { get; set; }
    }
}
