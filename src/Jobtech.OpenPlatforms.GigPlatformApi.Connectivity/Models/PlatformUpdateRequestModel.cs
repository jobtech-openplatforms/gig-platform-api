namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models
{
    public class PlatformUpdateRequestModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Webpage { get; set; } = "";
        public string PushNotificationUri { get; set; } = "";
        public string ExportDataUri { get; set; } = "";
    }
}