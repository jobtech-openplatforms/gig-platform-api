using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Models
{
    ///<summary>
    /// Request model to get UserAccessToken from the platform
    ///</summary>
    public class PlatformWebhookRequest
    {
        public string MyGigDataToken { get; set; }
        public string Email { get; set; }
    }
}
