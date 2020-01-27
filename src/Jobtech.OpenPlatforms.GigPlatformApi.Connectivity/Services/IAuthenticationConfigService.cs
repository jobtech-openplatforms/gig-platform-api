using System;
using System.Collections.Generic;
using System.Text;

namespace AF.Gig.WebApi.Services
{
    public interface IAuthenticationConfigService
    {
        string Token { get; }
        string AdminKey { get; }
        string ApiEndpointCreatePlatform { get; }
        string ApiEndpointValidateEmail { get; }
        string ApiEndpointCreateApplication { get; }
    }
}
