using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Models
{
    ///<summary>
    /// API model to request a connection between a User and a Platform
    ///</summary>
    public class ConnectionRequest
    {
        public string PlatformId { get; set; }
        public string UserId { get; set; }
    }
}
