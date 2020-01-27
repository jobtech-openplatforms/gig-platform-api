using AF.Gig.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Config
{
    public class GigDataServiceConfig
    {
        public string Token { get; set; }
        public string AdminKey { get; set; }
        public string ApiEndpointCreatePlatform { get; set; }
        public string ApiEndpointValidateEmail { get; set; }
        public string ApiEndpointCreateApplication { get; set; }
        public string ApiEndpointActivatePlatform { get; set; }
        public string ApiEndpointDeactivatePlatform { get; set; }
        public string ApiEndpointPlatformStatus { get; set; }
    }
}
