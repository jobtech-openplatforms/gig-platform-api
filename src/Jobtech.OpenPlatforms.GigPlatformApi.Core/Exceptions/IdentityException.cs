using System;
using System.Collections.Generic;
using System.Net;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Core.Exceptions
{
    public class IdentityException : Exception
    {

        public IdentityException(string message) :
            base(message)
        {
        }


    }
}