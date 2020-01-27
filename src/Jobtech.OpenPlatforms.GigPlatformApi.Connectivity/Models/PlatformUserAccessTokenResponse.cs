namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// Response model for getting the UserAccessToken from the platform
    ///</summary>
    public class PlatformUserAccessTokenResponse
    {
        public string UserId { get; set; }
        public string UserAccessToken { get; set; }
    }
}
