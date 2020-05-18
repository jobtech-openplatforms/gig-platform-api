namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// Request model to get user data from the platform
    ///</summary>
    public class UserDataRequest
    {
        public UserDataRequest(string platformToken, string username, string requestId)
        {
            PlatformToken = platformToken;
            Username = username;
            RequestId = requestId;
        }

        public string PlatformToken { get; }
        public string Username { get; }
        public string RequestId { get; }
    }
}