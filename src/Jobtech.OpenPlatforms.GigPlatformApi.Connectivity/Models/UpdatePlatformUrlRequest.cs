namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class UpdatePlatformUrlRequest
    {
        public string ProjectId { get; set; }
        public string Url { get; set; }
        public bool TestMode { get; set; }

    }
}