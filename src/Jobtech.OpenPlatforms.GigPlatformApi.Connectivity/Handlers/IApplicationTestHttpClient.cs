using System;
using System.Threading.Tasks;

namespace Jobtech.OpenPlatforms.GigPlatformApi.Connectivity.Handlers
{
    public interface IApplicationTestHttpClient
    {
        Task SendCallbackSuccess(Uri authCallbackUrl);
    }
}