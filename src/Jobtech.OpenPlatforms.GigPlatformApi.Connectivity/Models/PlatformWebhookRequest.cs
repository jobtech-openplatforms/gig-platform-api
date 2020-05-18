namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    ///<summary>
    /// Request model to get UserAccessToken from the platform
    ///</summary>
    public class PlatformWebhookRequest
    {
        public string PlatformToken { get; set; }
        public string Email { get; set; }
    }
}
