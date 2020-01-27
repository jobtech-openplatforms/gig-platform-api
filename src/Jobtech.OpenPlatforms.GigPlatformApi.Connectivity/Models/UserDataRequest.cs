namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// Request model to get user data from the platform
    ///</summary>
    public class UserDataRequest
    {
        public UserDataRequest(string myGigDataToken, string username, string requestId)
        {
            MyGigDataToken = myGigDataToken;
            Username = username;
            RequestId = requestId;
        }

        public string MyGigDataToken { get; }
        public string Username { get; }
        public string RequestId { get; }
    }
}