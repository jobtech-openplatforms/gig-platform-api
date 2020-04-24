using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Models.ApplicationModels
{
    public class ApplicationTestResponse
    {
        public string TestedUrl { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
