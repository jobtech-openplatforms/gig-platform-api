using System.Threading.Tasks;
using Jobtech.OpenPlatforms.GigDataCommon.Library.Messages;
using Rebus.Bus;

namespace Jobtech.OpenPlatforms.GigPlatformApi.EventDispatcher.Managers
{
    public class PlatformDispatchManager : IPlatformDispatchManager
    {

        private readonly IBus _bus;

        public PlatformDispatchManager(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendUserDataMessage(PlatformUserUpdateDataMessage platformUserDataMessage)
        {
            await _bus.Send(platformUserDataMessage);
        }
    }
}
