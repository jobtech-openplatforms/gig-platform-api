using Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Messages;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.PlatformModels
{
    public class PlatformUserDataResult
    {
        public string RequestId { get; set; }
        public string Username { get; set; }
        public string PlatformToken { get; set; }
        public PlatformDataFetchResult Result { get;  set; }
    }
}