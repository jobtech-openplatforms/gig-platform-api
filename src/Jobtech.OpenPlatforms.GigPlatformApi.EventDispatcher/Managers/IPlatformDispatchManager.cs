using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Messages;

namespace Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.Managers
{
    public interface IPlatformDispatchManager
    {
        Task SendUserDataMessage(PlatformUserUpdateDataMessage message);
    }
}
