namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// Request model to get user data from the platform
    ///</summary>
    public class UserDataRequest
    {
        public UserDataRequest(string platformToken, string userEmail, string requestId)
        {
            PlatformToken = platformToken;
            UserEmail = userEmail;
            RequestId = requestId;
        }

        public string PlatformToken { get; }
        public string UserEmail { get; }
        public string RequestId { get; }
    }
}