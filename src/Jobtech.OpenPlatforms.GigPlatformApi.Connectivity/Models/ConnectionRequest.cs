namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// API model to request a connection between a User and a Platform
    ///</summary>
    public class ConnectionRequest
    {
        public string PlatformId { get; set; }
        public string UserId { get; set; }
    }
}
