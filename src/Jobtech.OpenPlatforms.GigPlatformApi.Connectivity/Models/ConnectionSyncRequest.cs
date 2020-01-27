﻿using AF.GigPlatform.Core.ValueObjects;

namespace AF.GigPlatform.Connectivity.Models
{
    ///<summary>
    /// API model to request a sync of data between a User and a Platform
    ///</summary>
    public class ConnectionSyncRequest
    {
        public ConnectionId ConnectionId { get; set; }
    }
}
