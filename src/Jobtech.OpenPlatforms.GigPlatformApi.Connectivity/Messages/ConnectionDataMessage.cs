using AF.GigPlatform.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AF.GigPlatform.Connectivity.Messages
{
    public class ConnectionDataMessage
    {
        private ConnectionDataMessage() { }

        public ConnectionDataMessage(string userId, PlatformDataFetchResult result)
        {
            UserId = userId;
            Result = result;
        }

        public PlatformDataFetchResult Result { get; private set; }
        public string UserId { get; private set; }
    }
}
